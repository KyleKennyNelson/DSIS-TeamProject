--CONN ADMIN_OLS/123@localhost:15211/user_pdb;
--Drop policy incase you need
EXECUTE SA_SYSDBA.DROP_POLICY(policy_name => 'region_policy', drop_column => TRUE);

--Create policy and enable it
EXECUTE    SA_SYSDBA.CREATE_POLICY(policy_name => 'region_policy',column_name => 'region_label');
GRANT REGION_POLICY_DBA to ADMIN_OLS;
EXEC SA_SYSDBA.ENABLE_POLICY ('region_policy'); 
--Create levels, compartments and groups
-- label tag = sum(level)* 10 + sum(compartment)* 100 + sum(group) neu trung thi +10 moi lan
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',2000, 'L1', 'SINHVIEN');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',3000, 'L2', 'NHANVIENCOBAN');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',4000, 'L3', 'GIAOVU');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',5000, 'L4', 'GIANGVIEN');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',6000, 'L5', 'TRUONGDONVI');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',7000, 'L6', 'TRUONGKHOA');
--
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',10,'HTTT','HETHONGTHONGTIN');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',20,'CNPM','CONGNGHEPHANMEM');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',30,'KHMT','KHOAHOCMAYTINH');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',40,'CNTT','CONGNGHETHONGTIN');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',50,'TGMT','THIGIACMAYTINH');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',60,'MMT','MANGMAYTINH');
--
EXECUTE SA_COMPONENTS.CREATE_GROUP('region_policy',300,'TT','TRUNG TAM');
EXECUTE SA_COMPONENTS.CREATE_GROUP('region_policy',100,'CS1','CO SO 1', parent_name => 'TT');
EXECUTE SA_COMPONENTS.CREATE_GROUP('region_policy',200,'CS2','CO SO 2', parent_name => 'TT');

--create labels
--a) TRUONGKHOA
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '81300','L6:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:TT');
--b) truong bo mon CS2
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '71300','L5:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:TT');
--c) 1 GIAOVU
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '51300','L3:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:TT');
--d) data label: thong bao t1 danh cho tat ca truong bm
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '50000','L5::');
--e) data label: thong bao t2 danh cho tat ca sinh vien HTTT CS1
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '11100','L1:HTTT:CS1');
--f) data label: thong bao t3 danh cho truong bm KHMT CS1
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '53100','L5:KHMT:CS1');
--g) data label: thong bao t4 danh cho truong bm KHMT CS1 & CS2
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '53300','L5:KHMT:CS1,CS2');
--h) 3 chinh sach moi
--  h1) Sinh vien (ngoai tru sinh vien HTTT CS1) co the doc tat ca thong bao cho sinh vien trong chuyen nganh cua minh khong phan biet co so
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '13300','L1:KHMT:TT');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '12300','L1:CNPM:TT');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '11300','L1:HTTT:TT');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '15300','L1:TGMT:TT');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '16300','L1:MMT:TT');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '14300','L1:CNTT:TT');
--  h2) Giang vien co the doc tat ca thong bao trong don vi cua minh, rieng van phong khoa thi doc cua tat ca don vi
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '41100','L4:HTTT:CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '44100','L4:CNTT:CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '43100','L4:KHMT:CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '42100','L4:CNPM:CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '45100','L4:TGMT:CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '46100','L4:MMT:CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '61100','L4:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '41200','L4:HTTT:CS2');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '44200','L4:CNTT:CS2');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '43200','L4:KHMT:CS2');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '42200','L4:CNPM:CS2');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '45200','L4:TGMT:CS2');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '46200','L4:MMT:CS2');
--  h3) Nhan vien co ban co the doc tat ca thong bao danh cho nhan vien co ban, khong phan biet don vi, tai co so cua minh
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '41110','L2:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '41210','L2:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:CS2');

EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '10000','L1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '20000','L2');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '30000','L3');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '40000','L4');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '50000','L5');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '60000','L6');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '20100','L2::CS1');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '23100','L2:KHMT:CS2');
EXECUTE SA_LABEL_ADMIN.CREATE_LABEL('region_policy', '60200','L2::CS2');
--See all levels, compartments and groups
--SELECT * FROM DBA_SA_LEVELS;
--SELECT * FROM DBA_SA_COMPARTMENTS;
--SELECT * FROM DBA_SA_GROUPS;
--SELECT * FROM DBA_SA_GROUP_HIERARCHY; 
--Create table
drop table ADMIN.PROJECT_THONGBAO;
create table ADMIN.PROJECT_THONGBAO(
    ID_TB nvarchar2(5) PRIMARY KEY,
    NOIDUNG nvarchar2(1000)
);

--apply OLS policy
EXECUTE SA_POLICY_ADMIN.APPLY_TABLE_POLICY (policy_name => 'REGION_POLICY',schema_name => 'ADMIN',table_name => 'PROJECT_THONGBAO', table_options => 'LABEL_DEFAULT,READ_CONTROL', predicate => NULL);

--Insert test data and label
TRUNCATE TABLE ADMIN.PROJECT_THONGBAO;
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB001','thong bao t1 danh cho tat ca truong bm',CHAR_TO_LABEL('REGION_POLICY','L5::'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB002','thong bao t2 danh cho tat ca sinh vien HTTT CS1',CHAR_TO_LABEL('REGION_POLICY','L1:HTTT:CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB003','thong bao t3 danh cho truong bm KHMT CS1',CHAR_TO_LABEL('REGION_POLICY','L5:KHMT:CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB004','thong bao t4 danh cho truong bm KHMT CS1 va CS2',CHAR_TO_LABEL('REGION_POLICY','L5:KHMT:CS1,CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB005','thong bao danh cho giang vien o don vi BM HTTT CS1',CHAR_TO_LABEL('REGION_POLICY','L4:HTTT:CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB006','thong bao danh cho giang vien o don vi BM CNTT CS1',CHAR_TO_LABEL('REGION_POLICY','L4:CNTT:CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB007','thong bao danh cho giang vien o don vi BM HTTT CS2',CHAR_TO_LABEL('REGION_POLICY','L4:HTTT:CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB008','thong bao danh cho giang vien o don vi BM CNTT CS2',CHAR_TO_LABEL('REGION_POLICY','L4:CNTT:CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB009','thong bao danh cho giang vien o don vi BM CNPM CS2',CHAR_TO_LABEL('REGION_POLICY','L4:CNPM:CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB010','thong bao danh cho giang vien o don vi BM KHMT CS2',CHAR_TO_LABEL('REGION_POLICY','L4:KHMT:CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB011','thong bao danh cho giang vien o don vi BM KHMT CS1',CHAR_TO_LABEL('REGION_POLICY','L4:KHMT:CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB012','thong bao danh cho giang vien o don vi BM CNPM CS1',CHAR_TO_LABEL('REGION_POLICY','L4:CNPM:CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB013','thong bao danh cho giang vien o don vi BM TGMT CS2',CHAR_TO_LABEL('REGION_POLICY','L4:TGMT:CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB014','thong bao danh cho giang vien o don vi BM TGMT CS1',CHAR_TO_LABEL('REGION_POLICY','L4:TGMT:CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB015','thong bao danh cho giang vien o don vi BM MMTVT CS2',CHAR_TO_LABEL('REGION_POLICY','L4:MMT:CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB016','thong bao danh cho giang vien o don vi BM MTTVT CS1',CHAR_TO_LABEL('REGION_POLICY','L4:MMT:CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB017','thong bao danh cho giang vien o don vi VP KHOA',CHAR_TO_LABEL('REGION_POLICY','L4:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB018','thong bao danh cho tat ca sinh vien HTTT',CHAR_TO_LABEL('REGION_POLICY','L1:HTTT:TT'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB019','thong bao danh cho tat ca sinh vien CNPM',CHAR_TO_LABEL('REGION_POLICY','L1:CNPM:TT'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB020','thong bao danh cho tat ca sinh vien KHMT',CHAR_TO_LABEL('REGION_POLICY','L1:KHMT:TT'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB021','thong bao danh cho tat ca sinh vien TGMT',CHAR_TO_LABEL('REGION_POLICY','L1:TGMT:TT'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB022','thong bao danh cho tat ca sinh vien MMTVT',CHAR_TO_LABEL('REGION_POLICY','L1:MMT:TT'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB023','thong bao danh cho tat ca sinh vien CNTT',CHAR_TO_LABEL('REGION_POLICY','L1:CNTT:TT'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB024','thong bao danh cho tat ca sinh vien',CHAR_TO_LABEL('REGION_POLICY','L1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB025','thong bao danh cho tat ca nhan vien',CHAR_TO_LABEL('REGION_POLICY','L2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB026','thong bao danh cho tat ca giao vu',CHAR_TO_LABEL('REGION_POLICY','L3'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB027','thong bao danh cho tat ca giang vien',CHAR_TO_LABEL('REGION_POLICY','L4'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB028','thong bao danh cho tat ca truong don vi',CHAR_TO_LABEL('REGION_POLICY','L5'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB029','thong bao danh cho tat ca truong khoa',CHAR_TO_LABEL('REGION_POLICY','L6'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB030','thong bao danh cho tat ca nhan vien CS1',CHAR_TO_LABEL('REGION_POLICY','L2::CS1'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB031','thong bao danh cho tat ca nhan vien KHMT CS2',CHAR_TO_LABEL('REGION_POLICY','L2:KHMT:CS2'));
    insert into ADMIN.PROJECT_THONGBAO(ID_TB,NOIDUNG,REGION_LABEL) values ('TB032','thong bao danh cho tat ca nhan vien CS2',CHAR_TO_LABEL('REGION_POLICY','L2::CS2'));
COMMIT;

EXECUTE    SA_USER_ADMIN.SET_USER_LABELS('region_policy','ADMIN',max_read_label=>'L6:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:TT');
EXECUTE    SA_USER_ADMIN.SET_USER_LABELS('region_policy','ADMIN_OLS',max_read_label=>'L6:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:TT');
grant select on ADMIN.PROJECT_THONGBAO to PUBLIC;

--  a) gan nhan cho truong khoa
EXECUTE    SA_USER_ADMIN.SET_USER_LABELS('region_policy','NV003',max_read_label=>'L6:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:TT');

--  b) gan nhan cho tat ca truong bm tai CS2
declare
    cursor cur_TRGDONVI is (select * 
                            from ADMIN.PROJECT_NHANSU NS
                            inner join ADMIN.PROJECT_DONVI DV on NS.MANV = DV.TRGDV
                            where NS.VAITRO = 'TRGDONVI' and NS.COSO = 'CS2');
    result boolean;                        
begin
    for row_TRGDONVI in cur_TRGDONVI
    loop
        result := ADMIN.isUserExists(row_TRGDONVI.MANV);
        if(result = TRUE) then
            SA_USER_ADMIN.SET_USER_LABELS('region_policy',row_TRGDONVI.MANV,max_read_label=>'L5:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:TT');
        end if;
    end loop;
end;

--  c) gan nhan cho 1 giao vu
EXECUTE SA_USER_ADMIN.SET_USER_LABELS('region_policy','NV002',max_read_label=>'L3:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:TT');

--  h1)
declare
    cursor cur_SINHVIEN is (select * from ADMIN.PROJECT_SINHVIEN);
    result boolean;
    strLabel varchar2(100);
begin
    for row_SINHVIEN in cur_SINHVIEN
    loop
        result := ADMIN.isUserExists(row_SINHVIEN.MASV);
        if(result = TRUE) then
            strLabel := 'L1:' || row_SINHVIEN.MANGANH || ':TT';

            SA_USER_ADMIN.SET_USER_LABELS('region_policy',row_SINHVIEN.MASV,max_read_label=>strLabel);
        end if;
    end loop;
end;

--  h2)
declare
    cursor cur_GIANGVIEN is (select NS.MANV as MANV, DV.TENDV as TENDV, DV.COSO as COSO
                            from ADMIN.PROJECT_NHANSU NS
                            inner join ADMIN.PROJECT_DONVI DV on NS.DONVI = DV.MADV
                            where NS.VAITRO = 'GIANGVIEN');
    result boolean;     
    strLabel varchar2(100);
begin
    for row_GIANGVIEN in cur_GIANGVIEN
    loop
        result := ADMIN.isUserExists(row_GIANGVIEN.MANV);
        if(result = TRUE) then
            strLabel := 'L4:'; 
            
            if(row_GIANGVIEN.TENDV = 'VP KHOA') then
                strLabel := strLabel || 'HTTT,CNPM,KHMT,CNTT,TGMT,MMT:';
            elsif (row_GIANGVIEN.TENDV = 'BM HTTT') then
                strLabel := strLabel || 'HTTT:';
            elsif (row_GIANGVIEN.TENDV = 'BM CNTT') then
                strLabel := strLabel || 'CNTT:';
            elsif (row_GIANGVIEN.TENDV = 'BM KHMT') then
                strLabel := strLabel || 'KHMT:';
            elsif (row_GIANGVIEN.TENDV = 'BM CNPM') then
                strLabel := strLabel || 'CNPM:';
            elsif (row_GIANGVIEN.TENDV = 'BM TGMT') then
                strLabel := strLabel || 'TGMT:';
            elsif (row_GIANGVIEN.TENDV = 'BM MMTVT') then
                strLabel := strLabel || 'MMT:';
            end if;
            
            strLabel := strLabel || row_GIANGVIEN.COSO;
            
            SA_USER_ADMIN.SET_USER_LABELS('region_policy',row_GIANGVIEN.MANV,max_read_label=>strLabel);
        end if;
    end loop;
end;
--  h3)
declare
    cursor cur_NVCOBAN is (select *
                            from ADMIN.PROJECT_NHANSU NS
                            where NS.VAITRO = 'NVCOBAN');
    result boolean;     
    strLabel varchar2(100);
begin
    for row_NVCOBAN in cur_NVCOBAN
    loop
        result := ADMIN.isUserExists(row_NVCOBAN.MANV);
        if(result = TRUE) then
            strLabel := 'L2:HTTT,CNPM,KHMT,CNTT,TGMT,MMT:' || row_NVCOBAN.COSO;
            SA_USER_ADMIN.SET_USER_LABELS('region_policy',row_NVCOBAN.MANV,max_read_label=>strLabel);
        end if;
    end loop;
end;

--See all existing labels
--select * from ALL_SA_LABELS ORDER BY LABEL_TAG ASC;

--SELECT * FROM ALL_SA_USER_LABELS WHERE user_name = 'NV018';

--select * from ADMIN.PROJECT_THONGBAO