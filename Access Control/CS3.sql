--Tao role nhan vien giao vu
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
create role NVGIAOVU;
ALTER SESSION SET "_ORACLE_SCRIPT" = FALSE;

--Quyen nhan vien co ban
grant NVCOBAN to NVGIAOVU;

--Quyen insert va quyen update tren cac quan he sau
grant insert, update on ADMIN.PROJECT_SINHVIEN to NVGIAOVU;
grant insert, update on ADMIN.PROJECT_DONVI to NVGIAOVU;
grant insert, update on ADMIN.PROJECT_HOCPHAN to NVGIAOVU;
grant insert, update on ADMIN.PROJECT_KHMO to NVGIAOVU;

--Quyen select tren quan he PROJECT_PHANCONG
grant select on ADMIN.PROJECT_PHANCONG to NVGIAOVU;

--View cac dong phan cong lien quan cac hoc phan do van phong khoa phuj trach
CREATE OR REPLACE VIEW admin.UV_VPK_PHANCONG
AS 
    SELECT  PC.*
    FROM  PROJECT_DONVI  DV, PROJECT_HOCPHAN HP,PROJECT_PHANCONG PC
    WHERE dv.tendv = 'VP KHOA' AND dv.madv = HP.MADV AND HP.MAHP = pc.mahp;
    
--Grant quyen chinh sua tren bang phan cong cho giao vu
GRANT UPDATE ON ADMIN.UV_VPK_PHANCONG TO NVGIAOVU;

--VIEW CHINH SUA QUAN H? DANGKY
CREATE OR REPLACE VIEW admin.UV_GV_DANGKI
AS 
    SELECT  *
    FROM  PROJECT_DANGKI DK
    WHERE SYSDATE <= TO_DATE(DK.NAM ||'-' || DK.HK || '-15', 'YYYY-MM-DD') ;


--GRANT QUYEN DELETE V� INSERT TREN QUAN HE DANG KI
GRANT DELETE,INSERT ON ADMIN.UV_GV_DANGKI TO NVGIAOVU;

GRANT NVGIAOVU TO NV001;