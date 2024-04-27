--Tao role nhan vien giao vu
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
create role GIAOVU;
ALTER SESSION SET "_ORACLE_SCRIPT" = FALSE;

--Quyen nhan vien co ban
grant NVCOBAN to GIAOVU;

--Quyen insert va quyen update tren cac quan he sau
grant insert, update on ADMIN.PROJECT_SINHVIEN to GIAOVU;
grant insert, update on ADMIN.PROJECT_DONVI to GIAOVU;
grant insert, update on ADMIN.PROJECT_HOCPHAN to GIAOVU;
grant insert, update on ADMIN.PROJECT_KHMO to GIAOVU;

--Quyen select tren quan he PROJECT_PHANCONG
grant select on ADMIN.PROJECT_PHANCONG to GIAOVU;

--View cac dong phan cong lien quan cac hoc phan do van phong khoa phuj trach
CREATE OR REPLACE VIEW admin.UV_VPK_PHANCONG
AS 
    SELECT  PC.*
    FROM  PROJECT_DONVI  DV, PROJECT_HOCPHAN HP,PROJECT_PHANCONG PC
    WHERE dv.tendv = 'VP KHOA' AND dv.madv = HP.MADV AND HP.MAHP = pc.mahp;
    
--Grant quyen chinh sua tren bang phan cong cho giao vu
GRANT UPDATE ON ADMIN.UV_VPK_PHANCONG TO GIAOVU;

--VIEW CHINH SUA QUAN H? DANGKY
CREATE OR REPLACE VIEW admin.UV_GV_DANGKI
AS 
    SELECT  *
    FROM  PROJECT_DANGKI DK
    WHERE SYSDATE <= TO_DATE(DK.NAM ||'-' || DK.HK || '-15', 'YYYY-MM-DD') ;


--GRANT QUYEN DELETE V� INSERT TREN QUAN HE DANG KI
GRANT DELETE,INSERT ON ADMIN.UV_GV_DANGKI TO GIAOVU;

declare
    cursor cur_GIAOVU is (select * from ADMIN.PROJECT_NHANSU where VAITRO = 'GIAOVU');
    STRSQL varchar2(1000);
begin
    for row_GIAOVU in cur_GIAOVU
    loop
        STRSQL := 'CREATE USER ' || row_GIAOVU.MANV || ' identified by 123';
        EXECUTE IMMEDIATE (STRSQL);
        STRSQL := 'GRANT GIAOVU to ' || row_GIAOVU.MANV;
        EXECUTE IMMEDIATE (STRSQL);
    end loop;
end;