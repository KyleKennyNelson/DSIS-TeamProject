ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
create role GIANGVIEN;
--drop role GIANGVIEN;
grant NVCOBAN to GIANGVIEN;

create or replace view ADMIN.UV_GIANGVIEN_PHANCONG
as
    select *
    from ADMIN.PROJECT_PHANCONG
    where MAGV = SYS_CONTEXT('USERENV','SESSION_USER');
    
grant select on ADMIN.UV_GIANGVIEN_PHANCONG to GIANGVIEN;
grant select on ADMIN.PROJECT_DANGKI to GIANGVIEN;
grant UPDATE (DIEMTH,DIEMQT,DIEMCK,DIEMTK) on ADMIN.PROJECT_DANGKI to GIANGVIEN;

create or replace function SEC_GIANGVIEN_DANGKI_SEL_UPD(P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    cursor cur_GIANGVIEN is (
                                select VAITRO
                                from ADMIN.PROJECT_NHANSU
                                where MANV = SYS_CONTEXT('USERENV','SESSION_USER')
                            );
    cursor cur_PHANCONG is  (
                                select MAGV,MAHP,HK,NAM,MACT
                                from ADMIN.PROJECT_PHANCONG
                                where MAGV = SYS_CONTEXT('USERENV','SESSION_USER')
                            );
    vaitro varchar(40);
    STRSQL varchar2(2000);
begin
    open cur_GIANGVIEN;
    fetch cur_GIANGVIEN into vaitro;
    if (vaitro = 'GIANGVIEN') then
        begin
            STRSQL := '(';
            for row_PHANCONG in cur_PHANCONG
            loop
                STRSQL := STRSQL || '(  MAGV = ''' || row_PHANCONG.MAGV || ''' and MAHP = ''' || row_PHANCONG.MAHP || ''' and HK =  ' || to_char(row_PHANCONG.HK) || ' and NAM = ' || to_char(row_PHANCONG.NAM) || ' and MACT = ''' || row_PHANCONG.MACT || ''') 
                OR ';                     
            end loop;
            STRSQL := STRSQL || '1=0)';
            return STRSQL;
        end;
    else
        return '';
    end if;
    close cur_GIANGVIEN;
end;

BEGIN
    dbms_rls.add_policy(
        OBJECT_SCHEMA =>'ADMIN',
        OBJECT_NAME=>'PROJECT_DANGKI',
        POLICY_NAME =>'GIANGVIEN_DANGKI_SEL_UPD',
        FUNCTION_SCHEMA => 'ADMIN',
        POLICY_FUNCTION=>'SEC_GIANGVIEN_DANGKI_SEL_UPD',
        STATEMENT_TYPES=>'SELECT,UPDATE',
        UPDATE_CHECK=> TRUE
    );
END;