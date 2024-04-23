show parameter db_create_file_dest;
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
--Create policy
CONN ADMIN_OLS/123@localhost:15211/user_pdb;
BEGIN
    SA_SYSDBA.CREATE_POLICY(
    policy_name => 'region_policy',
    column_name => 'region_label'
    );
END;
--Enable policy
CONN ADMIN_OLS/123@localhost:15211/user_pdb;
EXEC SA_SYSDBA.ENABLE_POLICY ('region_policy'); 
--Create levels, compartments and groups
-- 10 - 20 - 30 - 40 - 60 - 80
CONN ADMIN_OLS/123@localhost:15211/user_pdb;
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',10, 'L1', 'SINHVIEN');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',20, 'L2', 'NHANVIENCOBAN');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',30, 'L3', 'GIAOVU');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',40, 'L4', 'GIANGVIEN');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',60, 'L5', 'TRUONGDONVI');
EXECUTE SA_COMPONENTS.CREATE_LEVEL('region_policy',80, 'L6', 'TRUONGKHOA');
--
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',20,'HTTT','HETHONGTHONGTIN');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',40,'CNPM','CONGNGHEPHANMEM');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',60,'KHMT','KHOAHOCMAYTINH');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',80,'CNTT','CONGNGHETHONGTIN');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',100,'TGMT','THIGIACMAYTINH');
EXECUTE SA_COMPONENTS.CREATE_COMPARTMENT('region_policy',120,'MMT','MANGMAYTINH');
--
EXECUTE SA_COMPONENTS.CREATE_GROUP('region_policy',40,'CS1','CO SO 1');
EXECUTE SA_COMPONENTS.CREATE_GROUP('region_policy',80,'CS2','CO SO 2');
--See all levels, compartments and groups
CONN ADMIN_OLS/123@localhost:15211/user_pdb;
SELECT * FROM DBA_SA_LEVELS;
SELECT * FROM DBA_SA_COMPARTMENTS;
SELECT * FROM DBA_SA_GROUPS;
SELECT * FROM DBA_SA_GROUP_HIERARCHY; 
--Create table
CONN ADMIN_OLS/123@localhost:15211/user_pdb;
--drop table ADMIN_OLS.PROJECT_THONGBAO;
create table ADMIN_OLS.PROJECT_THONGBAO(
    ID_TB nvarchar2(5) PRIMARY KEY,
    NOIDUNG nvarchar2(1000)
);

CONN ADMIN_OLS/123@localhost:15211/user_pdb;
BEGIN
 SA_POLICY_ADMIN.APPLY_TABLE_POLICY (
 POLICY_NAME => 'REGION_POLICY',
 SCHEMA_NAME => 'ADMIN_OLS',
 TABLE_NAME => 'PROJECT_THONGBAO',
 TABLE_OPTIONS => 'NO_CONTROL'
 );
END;
--Insert test data and label
insert into ADMIN_OLS.PROJECT_THONGBAO(ID_TB,NOIDUNG) values ('TB001','Thong bao danh cho truong phong');
insert into ADMIN_OLS.PROJECT_THONGBAO(ID_TB,NOIDUNG) values ('TB002','Thong bao danh cho sinh vien');

update ADMIN_OLS.PROJECT_THONGBAO set region_label = CHAR_TO_LABEL('REGION_POLICY','L6::CS1,CS2') where ID_TB = 'TB001';
update ADMIN_OLS.PROJECT_THONGBAO set region_label = CHAR_TO_LABEL('REGION_POLICY','L1') where ID_TB = 'TB002';
--Re-apply OLS policy
CONN ADMIN_OLS/123@localhost:15211/user_pdb;
BEGIN
SA_POLICY_ADMIN.REMOVE_TABLE_POLICY('REGION_POLICY','ADMIN_OLS','PROJECT_THONGBAO');
SA_POLICY_ADMIN.APPLY_TABLE_POLICY (
    policy_name => 'REGION_POLICY',
    schema_name => 'ADMIN_OLS',
    table_name => 'PROJECT_THONGBAO',
    table_options => 'READ_CONTROL',
    predicate => NULL
);
END;
--create role and users
create role TRUONGKHOA;
create role SINHVIEN;
grant connect to TRUONGKHOA,SINHVIEN;
grant select on ADMIN_OLS.PROJECT_THONGBAO to TRUONGKHOA,SINHVIEN;

create user NV001 identified by 123;
grant TRUONGKHOA to NV001;

create user SV001 identified by 123;
grant SINHVIEN to SV001;

--Set user labels
CONN ADMIN_OLS/123@localhost:15211/user_pdb;
BEGIN
    SA_USER_ADMIN.SET_USER_LABELS('region_policy','NV001','L6:CNTT:CS1');
    SA_USER_ADMIN.SET_USER_LABELS('region_policy','SV001','L1::CS2');
END;

--Test NV001 label L6 (TRUONGPHONG)
CONN NV001/123@localhost:15211/user_pdb;
select * from ADMIN_OLS.PROJECT_THONGBAO;
--Test SV001 label L1 (SINHVIEN)
CONN SV001/123@localhost:15211/user_pdb;
select * from ADMIN_OLS.PROJECT_THONGBAO;

--See all existing labels
select * from ALL_SA_LABELS;