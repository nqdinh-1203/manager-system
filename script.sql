/*
MASV: 19120478
HO TEN: Nguyễn Quang Định
LAB: 04
NGAY: 16/05/2022
*/

use QLSV
go

drop proc dbo.SP_INS_ENCRYPT_SINHVIEN;
go
create or alter proc SP_INS_ENCRYPT_SINHVIEN (@MaSV nvarchar(20), @HoTen nvarchar(100),
									  @NgaySinh datetime, @DiaChi nvarchar(200),
									  @MaLop varchar(20), @TenDN nvarchar(100), @Enc_MatKhau varchar(max))
as
	insert into SINHVIEN values (@MaSV, @HoTen, @NgaySinh, @DiaChi, @MaLop, @TenDN, convert(varchar(max),@Enc_MatKhau,1))
go

drop proc dbo.SP_INS_ENCRYPT_NHANVIEN;
go
create or alter proc SP_INS_ENCRYPT_NHANVIEN (@MaNV nvarchar(20), @HoTen nvarchar(100),
								 @Email varchar(20), @Enc_Luong varbinary(max),
								 @TenDN nvarchar(100), @Enc_MatKhau varchar(max))
as
	insert into NHANVIEN values (@MaNV, @HoTen, @Email, @Enc_Luong, @TenDN, convert(varchar(max),@Enc_MatKhau,1))
go


drop proc dbo.SP_SEL_ENCRYPT_NHANVIEN;
go
create or alter proc SP_SEL_ENCRYPT_NHANVIEN 
as
	select nv.MANV, nv.HOTEN, nv.EMAIL, convert(varchar(max),nv.LUONG,1)
	from NHANVIEN nv
go

exec SP_SEL_ENCRYPT_NHANVIEN;

exec SP_SEL_ENCRYPT_NHANVIEN;

drop proc dbo.SP_CHECK_LOGIN;
go
create or alter proc SP_CHECK_LOGIN (@TENDN nvarchar(100), @MATKHAU varchar(max))
as
	select * from dbo.SINHVIEN where TENDN = @TENDN and convert(varchar(max),MATKHAU,1) = @MATKHAU;
go