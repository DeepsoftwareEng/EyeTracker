create database EyeTracker;
use EyeTracker;
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
  TenLop varchar(50),
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
INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau) VALUES ('admin', 'password123');
INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau) VALUES ('teacher', 'teacher123');
INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau) VALUES ('student', 'student123');

INSERT INTO GiaoVien (MaGV, TenGV, NgaySinh, TenTaiKhoan) VALUES ('GV01', 'Nguyễn Văn A', '1980-01-01', 'teacher');
INSERT INTO GiaoVien (MaGV, TenGV, NgaySinh, TenTaiKhoan) VALUES ('GV02', 'Trần Thị B', '1981-02-02', 'teacher');

INSERT INTO Lop (MaLop, TenLop, GVCN) VALUES ('L01', 'Lớp 10A1', 'GV01');
INSERT INTO Lop (MaLop, TenLop, GVCN) VALUES ('L02', 'Lớp 10A2', 'GV02');

INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Nguyễn Văn C', '2005-03-03', '2023-09-01', 'Số 123 đường Lê Văn Lương', 1, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Trần Thị D', '2006-04-04', '2023-09-01', 'Số 456 đường Nguyễn Trãi', 2, 'L02', 'GV02');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Nguyễn Văn E', '2005-05-05', '2023-09-01', 'Số 789 đường Hoàng Quốc Việt', 1, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Trần Thị F', '2006-06-06', '2023-09-01', 'Số 1011 đường Giải Phóng', 2, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Lê Văn G', '2007-07-07', '2023-09-01', 'Số 1213 đường Trường Chinh', 3, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Phạm Thị H', '2008-08-08', '2023-09-01', 'Số 1415 đường Nguyễn Du', 4, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Nguyễn Thị I', '2009-09-09', '2023-09-01', 'Số 1617 đường Bà Triệu', 5, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Đỗ Thị J', '2010-10-10', '2023-09-01', 'Số 1819 đường Trần Hưng Đạo', 6, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Nguyễn Thị K', '2011-11-11', '2023-09-01', 'Số 2021 đường Điện Biên Phủ', 7, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Đào Thị L', '2012-12-12', '2023-09-01', 'Số 2223 đường Cách Mạng Tháng Tám', 8, 'L01', 'GV01');
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV) VALUES ('Nguyễn Thị M', '2013-01-01', '2023-09-01', 'Số 2425 đường Hùng Vương', 9, 'L01', 'GV01');