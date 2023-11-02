﻿create database EyeTracker;
use EyeTracker;
drop database EyeTracker
create table TaiKhoan(
  TenTaiKhoan varchar(50),
  MatKhau varchar(50),
  primary key(TenTaiKhoan)
);
create table GiaoVien(
  MaGV varchar(50),
  TenGV nvarchar(50),
  NgaySinh date,
  TenTaiKhoan varchar(50),
  primary key(MaGV),
  foreign key(TenTaiKhoan) references TaiKhoan(TenTaiKhoan)
);
create table Lop(
  MaLop varchar(50),
  TenLop nvarchar(50),
  GVCN varchar(50),
  primary key(MaLop),
  foreign key(GVCN) references GiaoVien(MaGV)
);
create table HocSinh(
  MaHocSinh int identity(1,1),
  HoTen nvarchar(50),
  NgaySinh date,
  NamNhapHoc date,
  DiaChi nvarchar(1000),
  DoCanThi float,
  MaLop varchar(50),
  MaGV varchar(50),
  primary key(MaHocSinh),
  foreign key(MaLop) references Lop(MaLop),
  foreign key(MaGV) references GiaoVien(MaGV)
);
select * from HocSinh;
select * from GiaoVien;
select * from Lop;
drop table HocSinh;
drop table Lop;
drop table GiaoVien;
-- Insert tài khoản
INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau)
VALUES ('admin', '123456');

-- Insert giáo viên
INSERT INTO GiaoVien (MaGV, TenGV, NgaySinh, TenTaiKhoan)
VALUES ('GV01', N'Nguyễn Văn A', '1980-03-08', 'admin');

-- Insert lớp
INSERT INTO Lop (MaLop, TenLop, GVCN)
VALUES ('L01', N'Lớp 10A1', 'GV01');

-- Insert học sinh
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV)
VALUES (N'Trần Văn B', '2006-05-06', '2023-08-15', N'Số 12, đường Nguyễn Trãi, quận 1, TP. Hồ Chí Minh', 8.0, 'L01', 'GV01'),
(N'Lê Thị C', '2006-07-07', '2023-08-15', N'Số 24, đường Lê Lợi, quận 1, TP. Hồ Chí Minh', 9.0, 'L01', 'GV01'),
(N'Đinh Văn D', '2006-08-08', '2023-08-15', N'Số 36, đường Pasteur, quận 1, TP. Hồ Chí Minh', 10.0, 'L01', 'GV01'),
(N'Nguyễn Thị E', '2006-09-09', '2023-08-15', N'Số 48, đường Trần Hưng Đạo, quận 1, TP. Hồ Chí Minh', 7.0, 'L01', 'GV01'),
(N'Phạm Văn F', '2006-10-10', '2023-08-15', N'Số 60, đường Võ Văn Tần, quận 1, TP. Hồ Chí Minh', 6.0, 'L01', 'GV01'),
(N'Trần Thị G', '2006-11-11', '2023-08-15', N'Số 72, đường Nguyễn Đình Chiểu, quận 1, TP. Hồ Chí Minh', 5.0, 'L01', 'GV01'),
(N'Lê Văn H', '2006-12-12', '2023-08-15', N'Số 84, đường Hai Bà Trưng, quận 1, TP. Hồ Chí Minh', 4.0, 'L01', 'GV01'),
(N'Đinh Thị I', '2007-01-01', '2023-08-15', N'Số 96, đường Nguyễn Huệ, quận 1, TP. Hồ Chí Minh', 3.0, 'L01', 'GV01'),
(N'Nguyễn Văn J', '2007-02-02', '2023-08-15', N'Số 108, đường Cách Mạng Tháng Tám, quận 1, TP. Hồ Chí Minh', 2.0, 'L01', 'GV01'),
(N'Phạm Thị K', '2007-03-03', '2023-08-15', N'Số 120, đường Điện Biên Phủ, quận 1, TP. Hồ Chí Minh', 1.0, 'L01', 'GV01');