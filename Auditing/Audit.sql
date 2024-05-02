CREATE AUDIT POLICY audit_obj_pol_dangki
ACTIONS ALL ON admin.project_dangki;
AUDIT POLICY audit_obj_pol_dangki EXCEPT admin whenever not successful;

CREATE AUDIT POLICY audit_obj_pol_phancong
ACTIONS ALL ON admin.project_phancong;

AUDIT POLICY audit_obj_pol_phancong;

CREATE AUDIT POLICY audit_obj_pol_SINHVIEN
ACTIONS ALL ON admin.PROJECT_SINHVIEN;
AUDIT POLICY audit_obj_pol_SINHVIEN;

CREATE AUDIT POLICY audit_obj_pol_NHANSU
ACTIONS ALL ON admin.PROJECT_NHANSU;
AUDIT POLICY audit_obj_pol_NHANSU;

CREATE AUDIT POLICY audit_obj_pol_KHMO
ACTIONS ALL ON admin.PROJECT_KHMO;
AUDIT POLICY audit_obj_pol_KHMO;

CREATE AUDIT POLICY audit_obj_pol_HOCPHAN
ACTIONS ALL ON admin.PROJECT_HOCPHAN;
AUDIT POLICY audit_obj_pol_HOCPHAN;

CREATE AUDIT POLICY audit_obj_pol_DONVI
ACTIONS ALL ON admin.PROJECT_DONVI;
AUDIT POLICY audit_obj_pol_DONVI;

--3-a
CREATE OR REPLACE FUNCTION sys.check_giangvien_role
RETURN VARCHAR2
IS
  v_role VARCHAR2(50);
BEGIN
  -- Ki?m tra xem ng??i dùng hi?n t?i có vai trò "GIANGVIEN" hay không
  SELECT 'GIANGVIEN' INTO v_role
  FROM DBA_ROLE_PRIVS
  WHERE GRANTEE = SYS_CONTEXT('USERENV', 'SESSION_USER')
  AND GRANTED_ROLE = 'GIANGVIEN';

  -- Tr? v? vai trò "GIANGVIEN" n?u có, ng??c l?i tr? v? NULL
  RETURN v_role;
EXCEPTION
  WHEN NO_DATA_FOUND THEN
    RETURN NULL;
END;


/
BEGIN
  DBMS_FGA.add_policy(
    object_schema   => 'admin',
    object_name     => 'project_dangki',
    policy_name     => 'audit_policy_project_dangki',
    audit_condition => 'check_giangvien_role() = NULL',
    statement_types => 'UPDATE, INSERT, DELETE',
    audit_column    => 'DIEMTH, DIEMQT, DIEMCK, DIEMTK',
    audit_trail     => DBMS_FGA.DB,
    enable          => TRUE
  );
END;

/
--3-b
CREATE OR REPLACE FUNCTION f_username RETURN VARCHAR2 IS
  USERROLE VARCHAR2(50);
BEGIN
  USERROLE := SYS_CONTEXT('USERENV', 'SESSION_USER');
  RETURN USERROLE;
END ;
/
BEGIN
  DBMS_FGA.ADD_POLICY(
    object_schema   => 'ADMIN',  
    object_name     => 'project_NHANSU', 
    policy_name     => 'audit_policy_project_nhansu',
    audit_condition => 'MANV != f_username()',
    statement_types => 'SELECT', 
    audit_column    => 'PHUCAP',  
    audit_trail     => DBMS_FGA.DB + DBMS_FGA.EXTENDED,
    enable          => TRUE
  );
END;
/
SELECT * FROM unified_audit_trail;

SELECT VALUE FROM V$OPTION WHERE PARAMETER = 'Unified Auditing';