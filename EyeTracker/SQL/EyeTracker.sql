create database EyeTracker;
use EyeTracker;
drop database EyeTracker
create table TaiKhoan(
  TenTaiKhoan varchar(50),
  MatKhau varchar(50),
  Email varchar(50),
  ChucVu varchar(10),
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
select * from TaiKhoan;
select * from HocSinh;
select * from GiaoVien;
select * from Lop;
drop table HocSinh;
drop table Lop;
drop table GiaoVien;
-- Insert tài khoản
insert into TaiKhoan values ('','','teacher')
INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau, ChucVu)
VALUES ('admin', '123456', 'admin');
INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau, ChucVu)
VALUES ('gv1', '1234', 'Teacher');
INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau, ChucVu)
VALUES ('gv0', '', 'Teacher');
insert into TaiKhoan values('gv2','pass2','Teacher'),
						   ('gv3','pass3','Teacher')
-- Insert giáo viên
INSERT INTO GiaoVien (MaGV, TenGV, NgaySinh, TenTaiKhoan)
VALUES ('GV00', N'', '', 'gv0');
insert into GiaoVien values ('GV01',N'Nguyễn Quang Hưng','2002-7-25','gv1' ),
							('GV02',N'Nguyễn Phúc Hưng','2002-7-25','gv2' ),
						    ('GV03',N'Phạm Hoàng Tiến','2002-1-1','gv3' )
-- Insert lớp
INSERT INTO Lop (MaLop, TenLop, GVCN)
VALUES ('L01', N'Lớp 10A1', 'GV01');
insert into Lop values ('L02',N'Lớp 10A2','GV02'),
				('L03',N'Lớp 10A3','GV03')
-- Insert học sinh
INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV)
VALUES (N'Trần Văn B', '2006-05-06', '2023-08-15', N'Số 12, đường Nguyễn Trãi, quận 1, TP. Hồ Chí Minh', 8.0, 'L01', 'GV02'),
(N'Lê Thị C', '2006-07-07', '2023-08-15', N'Số 24, đường Lê Lợi, quận 1, TP. Hồ Chí Minh', 9.0, 'L01', 'GV02'),
(N'Đinh Văn D', '2006-08-08', '2023-08-15', N'Số 36, đường Pasteur, quận 1, TP. Hồ Chí Minh', 10.0, 'L01', 'GV02'),
(N'Nguyễn Thị E', '2006-09-09', '2023-08-15', N'Số 48, đường Trần Hưng Đạo, quận 1, TP. Hồ Chí Minh', 7.0, 'L01', 'GV02'),
(N'Phạm Văn F', '2006-10-10', '2023-08-15', N'Số 60, đường Võ Văn Tần, quận 1, TP. Hồ Chí Minh', 6.0, 'L01', 'GV02'),
(N'Trần Thị G', '2006-11-11', '2023-08-15', N'Số 72, đường Nguyễn Đình Chiểu, quận 1, TP. Hồ Chí Minh', 5.0, 'L01', 'GV02'),
(N'Lê Văn H', '2006-12-12', '2023-08-15', N'Số 84, đường Hai Bà Trưng, quận 1, TP. Hồ Chí Minh', 4.0, 'L01', 'GV02'),
(N'Đinh Thị I', '2007-01-01', '2023-08-15', N'Số 96, đường Nguyễn Huệ, quận 1, TP. Hồ Chí Minh', 3.0, 'L01', 'GV02'),
(N'Nguyễn Văn J', '2007-02-02', '2023-08-15', N'Số 108, đường Cách Mạng Tháng Tám, quận 1, TP. Hồ Chí Minh', 2.0, 'L01', 'GV02'),
(N'Phạm Thị K', '2007-03-03', '2023-08-15', N'Số 120, đường Điện Biên Phủ, quận 1, TP. Hồ Chí Minh', 1.0, 'L01', 'GV02');

INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV)
VALUES (N'Trần Văn A', '2003-05-06', '2023-08-15', N'Số 12, đường Nguyễn Trãi, quận 1, TP. Hồ Chí Minh',0.75, 'L02', 'GV03'),
(N'Lê Thị B', '2003-07-07', '2023-08-15', N'Số 24, đường Lê Lợi, quận 1, TP. Hồ Chí Minh', 4, 'L02', 'GV03'),
(N'Đinh Văn C', '2003-08-08', '2023-08-15', N'Số 36, đường Pasteur, quận 1, TP. Hồ Chí Minh', 3.5, 'L02', 'GV03'),
(N'Nguyễn Thị D', '2003-09-09', '2023-08-15', N'Số 48, đường Trần Hưng Đạo, quận 1, TP. Hồ Chí Minh', 2, 'L02', 'GV03'),
(N'Phạm Văn E', '2003-10-10', '2023-08-15', N'Số 60, đường Võ Văn Tần, quận 1, TP. Hồ Chí Minh', 1.75, 'L02', 'GV03'),
(N'Trần Thị F', '2003-11-11', '2023-08-15', N'Số 72, đường Nguyễn Đình Chiểu, quận 1, TP. Hồ Chí Minh', 3.25, 'L02', 'GV03'),
(N'Lê Văn G', '2003-12-12', '2023-08-15', N'Số 84, đường Hai Bà Trưng, quận 1, TP. Hồ Chí Minh', 3, 'L02', 'GV03'),
(N'Đinh Thị H', '2003-01-01', '2023-08-15', N'Số 96, đường Nguyễn Huệ, quận 1, TP. Hồ Chí Minh', 2.75, 'L02', 'GV03'),
(N'Nguyễn Văn I', '2003-02-02', '2023-08-15', N'Số 108, đường Cách Mạng Tháng Tám, quận 1, TP. Hồ Chí Minh', 2.25, 'L02', 'GV03'),
(N'Phạm Thị J', '2003-03-03', '2023-08-15', N'Số 120, đường Điện Biên Phủ, quận 1, TP. Hồ Chí Minh', 0, 'L02', 'GV03');

INSERT INTO HocSinh (HoTen, NgaySinh, NamNhapHoc, DiaChi, DoCanThi, MaLop, MaGV)
VALUES (N'Trần Văn Z', '2003-05-06', '2023-08-15', N'Số 12, đường Nguyễn Trãi, quận 1, TP. Hồ Chí Minh',0.75, 'L03', 'GV01'),
(N'Lê Thị X', '2003-07-07', '2023-08-15', N'Số 24, đường Lê Lợi, quận 1, TP. Hồ Chí Minh', 4, 'L03', 'GV01'),
(N'Đinh Văn C', '2003-08-08', '2023-08-15', N'Số 36, đường Pasteur, quận 1, TP. Hồ Chí Minh', 3.5, 'L03', 'GV01'),
(N'Nguyễn Thị V', '2003-09-09', '2023-08-15', N'Số 48, đường Trần Hưng Đạo, quận 1, TP. Hồ Chí Minh', 2, 'L03', 'GV01'),
(N'Phạm Văn B', '2003-10-10', '2023-08-15', N'Số 60, đường Võ Văn Tần, quận 1, TP. Hồ Chí Minh', 1.75, 'L03', 'GV01'),
(N'Trần Thị N', '2003-11-11', '2023-08-15', N'Số 72, đường Nguyễn Đình Chiểu, quận 1, TP. Hồ Chí Minh', 3.25, 'L03', 'GV01'),
(N'Lê Văn M', '2003-12-12', '2023-08-15', N'Số 84, đường Hai Bà Trưng, quận 1, TP. Hồ Chí Minh', 3, 'L03', 'GV01'),
(N'Đinh Thị L', '2003-01-01', '2023-08-15', N'Số 96, đường Nguyễn Huệ, quận 1, TP. Hồ Chí Minh', 2.75, 'L03', 'GV01'),
(N'Nguyễn Văn K', '2003-02-02', '2023-08-15', N'Số 108, đường Cách Mạng Tháng Tám, quận 1, TP. Hồ Chí Minh', 2.25, 'L03', 'GV01'),
(N'Phạm Thị P', '2003-03-03', '2023-08-15', N'Số 120, đường Điện Biên Phủ, quận 1, TP. Hồ Chí Minh', 0, 'L03', 'GV01');
select Top 1 * from GiaoVien Order By MaGV DESC
select COUNT(*) from Lop
select Count(*) from HocSinh
select * from GiaoVien where MaGv != 'GV00' and ChucVu != 'admin'
update TaiKhoan set ChucVu = 'Admin' where TenTaiKhoan = 'admin'
select * from TaiKhoan where TenTaiKhoan != '' and TenTaiKhoan != 'gv0'
select * from GiaoVien