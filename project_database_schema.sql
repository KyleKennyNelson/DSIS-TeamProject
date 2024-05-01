--alter session set "_oracle_script"=true;
-- drop user ADMIN CASCADE;
create user ADMIN identified by 123;
grant SYSDBA to ADMIN;
grant DBA to ADMIN;
/
CREATE OR REPLACE FUNCTION ADMIN.isUserExists(pv_user IN varchar2)
return BOOLEAN
IS
ld_dummy date;
CURSOR lcur_usr_xsts IS
SELECT created FROM all_users
WHERE username = upper(pv_user);
BEGIN
    OPEN lcur_usr_xsts;
    FETCH lcur_usr_xsts INTO ld_dummy;
    IF lcur_usr_xsts%NOTFOUND THEN
--do_something;
        CLOSE lcur_usr_xsts;
        return FALSE;
    ELSE
        CLOSE lcur_usr_xsts;
        return TRUE;
    END IF;
END;
/
--Drop PDB incase you need
--alter pluggable database user_pdb close immediate instances=all;
--alter session set container=CDB$ROOT;
--drop pluggable database user_pdb INCLUDING DATAFILES;
--Create pluggable database
create pluggable database user_pdb  admin user user_pdb identified by 123 roles=(connect) file_name_convert = ('/oradata','/user_pdb/');
alter pluggable database user_pdb open;
--See PDB status
select name,open_mode from v$pdbs;
--Alter container to a pdb
alter session set container=user_pdb;
--Check if OLS is enabled
SELECT VALUE FROM v$option WHERE parameter = 'Oracle Label Security';
SELECT status FROM dba_ols_status WHERE name = 'OLS_CONFIGURE_STATUS'; 
--Enable OLS
EXEC LBACSYS.CONFIGURE_OLS;
EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;
--Restart database
SHUTDOWN IMMEDIATE;
STARTUP;
--Check if OLS is enabled
select * from v$services; 
--Run in CDB to see all CDB names in case you don't know
SELECT name, pdb
FROM   v$services
ORDER BY name;
--switch to root container and unlock lbacsys
alter session set container=CDB$ROOT;
ALTER USER lbacsys IDENTIFIED BY lbacsys ACCOUNT UNLOCK CONTAINER=ALL; 
--Open read write for PDB in case it is not openned
ALTER PLUGGABLE DATABASE user_pdb OPEN READ WRITE; 
--Back to PDB
alter session set container=user_pdb;
SHOW CON_NAME;
--drop user ADMIN_OLS CASCADE;
--Create user ADMIN_OLS
CREATE USER ADMIN_OLS IDENTIFIED BY 123 CONTAINER = CURRENT;
GRANT SYSDBA TO ADMIN_OLS;
GRANT CONNECT,RESOURCE TO ADMIN_OLS;
GRANT UNLIMITED TABLESPACE TO ADMIN_OLS;
GRANT SELECT ANY DICTIONARY TO ADMIN_OLS; 
--Grant all necessary execute priviledges
GRANT EXECUTE ON LBACSYS.SA_COMPONENTS TO ADMIN_OLS WITH GRANT OPTION;
GRANT EXECUTE ON LBACSYS.sa_user_admin TO ADMIN_OLS WITH GRANT OPTION;
GRANT EXECUTE ON LBACSYS.sa_label_admin TO ADMIN_OLS WITH GRANT OPTION;
GRANT EXECUTE ON sa_policy_admin TO ADMIN_OLS WITH GRANT OPTION;
GRANT EXECUTE ON char_to_label TO ADMIN_OLS WITH GRANT OPTION; 
--Grant lbac_dba and execute priviledges to ADMIN_OLS
GRANT LBAC_DBA TO ADMIN_OLS;
GRANT EXECUTE ON sa_sysdba TO ADMIN_OLS;
GRANT EXECUTE ON TO_LBAC_DATA_LABEL TO ADMIN_OLS;
GRANT DBA to ADMIN_OLS;
grant execute on ADMIN.isUserExists to ADMIN_OLS;

create table ADMIN.PROJECT_NHANSU(
    MANV varchar2(5),
    HOTEN VARCHAR2(100),
    PHAI VARCHAR(10),
    NGSINH DATE,
    PHUCAP NUMBER,
    DT VARCHAR(12),
    VAITRO VARCHAR2(40),
    DONVI varchar2(5),
    COSO varchar2(10),
    PRIMARY KEY(MANV)
);
--drop table ADMIN.PROJECT_SINHVIEN;
create table ADMIN.PROJECT_SINHVIEN(
    MASV varchar2(5),
    HOTEN VARCHAR(100),
    PHAI VARCHAR(10),
    NGSINH DATE,
    DIACHI VARCHAR(200),
    DT VARCHAR(12),
    MACT varchar2(10),
    MANGANH varchar2(10),
    COSO varchar2(10),
    SOTCTL NUMBER,
    DTBTL FLOAT,
    PRIMARY KEY(MASV)
);
--drop table ADMIN.PROJECT_DONVI;
create table ADMIN.PROJECT_DONVI(
    MADV varchar2(5),
    TENDV VARCHAR(200),
    TRGDV varchar2(5),
    COSO varchar2(10),
    PRIMARY KEY(MADV)
);

ALTER TABLE ADMIN.PROJECT_NHANSU ADD CONSTRAINT FK_NHANSU_DONVI FOREIGN KEY(DONVI) REFERENCES ADMIN.PROJECT_DONVI(MADV);
--drop table ADMIN.PROJECT_HOCPHAN;
create table ADMIN.PROJECT_HOCPHAN (
    MAHP varchar2(5),
    TENHP VARCHAR(200),
    SOTC NUMBER,
    STLT NUMBER,
    STTH NUMBER,
    SOSVTD NUMBER,
    MADV varchar2(5),
    PRIMARY KEY(MAHP)
);

ALTER TABLE ADMIN.PROJECT_HOCPHAN ADD CONSTRAINT FK_HOCPHAN_DONVI FOREIGN KEY(MADV) REFERENCES ADMIN.PROJECT_DONVI(MADV);
--drop table ADMIN.PROJECT_KHMO;
create table ADMIN.PROJECT_KHMO(
    MAHP varchar2(5),
    HK NUMBER,
    NAM NUMBER,
    MACT VARCHAR(10),
    PRIMARY KEY(MAHP,HK,NAM,MACT)
);

ALTER TABLE ADMIN.PROJECT_KHMO ADD CONSTRAINT FK_KHMO_HOCPHAN FOREIGN KEY(MAHP) REFERENCES ADMIN.PROJECT_HOCPHAN(MAHP);
--drop table ADMIN.PROJECT_PHANCONG;
create table ADMIN.PROJECT_PHANCONG(
    MAGV varchar2(5),
    MAHP varchar2(5),
    HK NUMBER,
    NAM NUMBER,
    MACT VARCHAR(10),
    PRIMARY KEY(MAGV,MAHP,HK,NAM,MACT)
);

ALTER TABLE ADMIN.PROJECT_PHANCONG ADD CONSTRAINT FK_PHANCONG_NHANSU FOREIGN KEY(MAGV) REFERENCES ADMIN.PROJECT_NHANSU(MANV);
ALTER TABLE ADMIN.PROJECT_PHANCONG ADD CONSTRAINT FK_PHANCONG_KHMO FOREIGN KEY(MAHP,HK,NAM,MACT) REFERENCES ADMIN.PROJECT_KHMO(MAHP,HK,NAM,MACT);

--drop table ADMIN.PROJECT_DANGKI;
create table ADMIN.PROJECT_DANGKI(
    MASV varchar2(5),
    MAGV varchar2(5),
    MAHP varchar2(5),
    HK NUMBER,
    NAM NUMBER,
    MACT VARCHAR(10),
    DIEMTH FLOAT,
    DIEMQT FLOAT,
    DIEMCK FLOAT,
    DIEMTK FLOAT,
    
    PRIMARY KEY(MASV,MAGV,MAHP,HK,NAM,MACT)
);

ALTER TABLE ADMIN.PROJECT_DANGKI ADD CONSTRAINT FK_DANGKI_KHMO FOREIGN KEY(MAHP,HK,NAM,MACT) REFERENCES ADMIN.PROJECT_KHMO(MAHP,HK,NAM,MACT);
ALTER TABLE ADMIN.PROJECT_DANGKI ADD CONSTRAINT FK_DANGKI_NHANSU FOREIGN KEY(MAGV) REFERENCES ADMIN.PROJECT_NHANSU(MANV);
ALTER TABLE ADMIN.PROJECT_DANGKI ADD CONSTRAINT FK_DANGKI_SINHVIEN FOREIGN KEY(MASV) REFERENCES ADMIN.PROJECT_SINHVIEN(MASV);