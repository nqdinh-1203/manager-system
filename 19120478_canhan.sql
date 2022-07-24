/*
MASV: 19120478
HO TEN: Nguyễn Quang Định
LAB: 03
NGAY: 16/04/2022
*/

--TAO DB
create database QLSV
--TAO TABLE
use QLSV
go

drop table SINHVIEN
create table SINHVIEN
(
	MASV varchar(20),
	HOTEN nvarchar(100) not null,
	NGAYSINH datetime,
	DIACHI nvarchar(200),
	MALOP varchar(20),
	TENDN nvarchar(100) not null,
	MATKHAU varbinary(max) not null
	primary key (MASV)
)

drop table NHANVIEN
create table NHANVIEN
(
	MANV varchar(20),
	HOTEN nvarchar(100) not null,
	EMAIL varchar(20),
	LUONG varbinary(max),
	TENDN nvarchar(100) not null,
	MATKHAU varbinary(max) not null
	primary key (MANV)
)

drop table LOP
create table LOP
(
	MALOP varchar(20),
	TENLOP nvarchar(100) not null,
	MANV varchar(20)
	primary key (MALOP)
)

alter table LOP add constraint fk_lop_nv foreign key (MANV) references NHANVIEN(MANV)
alter table SINHVIEN add constraint fk_sv_lop foreign key (MALOP) references LOP(MALOP)

--Tao symmetric key
create symmetric key KEY_01
with algorithm = aes_256
encryption by password = '19120478'
go

use QLSV
go

-- proc Them Sinh vien
drop proc dbo.SP_INS_SINHVIEN;
go
create or alter proc SP_INS_SINHVIEN (@MaSV nvarchar(20), @HoTen nvarchar(100),
									  @NgaySinh datetime, @DiaChi nvarchar(200),
									  @MaLop varchar(20), @TenDN nvarchar(100), @MatKhau varchar(max))
as
	insert into SINHVIEN values (@MaSV, @HoTen, @NgaySinh, @DiaChi, @MaLop, @TenDN,
								CONVERT(varbinary, hashbytes('MD5', @MatKhau)))
go

EXEC SP_INS_SINHVIEN 'SV01', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', 'CNTT-K35', 'NVA', '123456'
EXEC SP_INS_SINHVIEN 'SV02', 'NGUYEN QUANG DINH', '12/03/2001', '290 AN DUONG VUONG', 'CNTT-K19', 'NQDINH', 'DINH123'
go

-- proc Them Nhan vien
drop proc dbo.SP_INS_NHANVIEN;
go
create or alter proc SP_INS_NHANVIEN (@MaNV nvarchar(20), @HoTen nvarchar(100),
								 @Email varchar(20), @Luong int,
								 @TenDN nvarchar(100), @MatKhau varchar(max))
as
	open symmetric key KEY_01
	decryption by password = '19120478'

	declare @Encripted_Luong varbinary(max)
	set @Encripted_Luong = ENCRYPTBYKEY(key_guid('KEY_01'),CONVERT(varchar, @Luong))
	insert into NHANVIEN values (@MaNV, @HoTen, @Email, @Encripted_Luong, @TenDN, CONVERT(varbinary, hashbytes('SHA1', @MatKhau)))

	close symmetric key KEY_01
go

EXEC SP_INS_NHANVIEN 'NV01', 'NGUYEN VAN A', 'NVA@', 3000000, 'NVA', 'abcd12'
EXEC SP_INS_NHANVIEN 'NV02', 'LE THI B', 'LTB@', 5000000, 'LTB', 'abc123'
go


-- Proc Select Nhan vien
drop proc dbo.SP_SEL_NHANVIEN;
go
create or alter proc SP_SEL_NHANVIEN 
as
	open symmetric key KEY_01
	decryption by password = '19120478'

	select nv.MANV, nv.HOTEN, nv.EMAIL, CONVERT(varchar,DECRYPTBYKEY(nv.LUONG)) as LUONGNV
	from NHANVIEN nv

	close symmetric key KEY_01
go

exec SP_SEL_NHANVIEN 
go

-- procedure kiem tra login trong chuong trinh
use QLSV
go

drop proc dbo.SP_CHECK_LOGIN_SV;
go
create or alter proc SP_CHECK_LOGIN_SV (@TENDN nvarchar(100), @MATKHAU varchar(max))
as
	select * from dbo.SINHVIEN where TENDN = @TENDN and MATKHAU = CONVERT(varbinary, hashbytes('MD5', @MATKHAU));
go

drop proc dbo.SP_CHECK_LOGIN_NV;
go
create or alter proc SP_CHECK_LOGIN_NV (@TENDN nvarchar(100), @MATKHAU varchar(max))
as
	select * from dbo.NHANVIEN where TENDN = @TENDN and MATKHAU = CONVERT(varbinary, hashbytes('SHA1', @MATKHAU));
go


exec SP_CHECK_LOGIN_NV 'NVA', '123456';
go

exec SP_CHECK_LOGIN_SV 'NVA', '123456';
go