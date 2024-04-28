ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
create role SINHVIEN;
grant connect to SINHVIEN;

grant select on ADMIN.PROJECT_SINHVIEN to SINHVIEN;
grant update (DIACHI,DT) on ADMIN.PROJECT_SINHVIEN to SINHVIEN;


--run with SYSDBA privilege
--connect sys/123@localhost:15211 as SYSDBA;
create or replace function sys.SEC_SINHVIEN_SINHVIEN_SEL_UPD(P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    cursor cur is   (
                        select GRANTED_ROLE 
                        from sys.DBA_ROLE_PRIVS 
                        where GRANTEE = SYS_CONTEXT('USERENV','SESSION_USER') and GRANTED_ROLE = 'SINHVIEN'
                    );
    vaitro varchar(40);
    masv varchar2(5);
begin
    open cur;
    fetch cur into vaitro;
    if (cur%FOUND) then
        masv := SYS_CONTEXT('USERENV','SESSION_USER');
        return 'MASV = ''' || masv || '''';
    else
        return '';
    end if;
end;



BEGIN
    dbms_rls.add_policy(
        OBJECT_SCHEMA =>'ADMIN',
        OBJECT_NAME=>'PROJECT_SINHVIEN',
        POLICY_NAME =>'SINHVIEN_SINHVIEN_SEL_UPD',
        FUNCTION_SCHEMA => 'sys',
        POLICY_FUNCTION=>'SEC_SINHVIEN_SINHVIEN_SEL_UPD',
        STATEMENT_TYPES=>'SELECT,UPDATE',
        UPDATE_CHECK=> TRUE
    );
END;

--connect sys/123@localhost:15211 as SYSDBA;
create or replace function sys.SEC_SINHVIEN_KHMO_SEL(P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    cursor cur is   (
                        select GRANTED_ROLE 
                        from sys.DBA_ROLE_PRIVS 
                        where GRANTEE = SYS_CONTEXT('USERENV','SESSION_USER') and GRANTED_ROLE = 'SINHVIEN'
                    );
    cursor cur_SINHVIEN is  (
                                select MACT
                                from ADMIN.PROJECT_SINHVIEN
                                where MASV = SYS_CONTEXT('USERENV','SESSION_USER')
                            );
    vaitro varchar(40);
    mact varchar(10);
    masv varchar2(5);
begin
    open cur;
    fetch cur into vaitro;
    if (cur%FOUND) then
        open cur_SINHVIEN;
        fetch cur_SINHVIEN into mact;
        close cur;
        masv := SYS_CONTEXT('USERENV','SESSION_USER');
        return 'MACT = ''' || mact || '''';
    else
        return '';
    end if;
end;

grant select on ADMIN.PROJECT_KHMO to SINHVIEN;

BEGIN
    dbms_rls.add_policy(
        OBJECT_SCHEMA =>'ADMIN',
        OBJECT_NAME=>'PROJECT_KHMO',
        POLICY_NAME =>'SSINHVIEN_KHMO_SEL',
        FUNCTION_SCHEMA => 'sys',
        POLICY_FUNCTION=>'SEC_SINHVIEN_KHMO_SEL',
        STATEMENT_TYPES=>'SELECT'
    );
END;

--connect sys/123@localhost:15211 as SYSDBA;
create or replace function sys.SEC_SINHVIEN_HOCPHAN_SEL(P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    cursor cur is   (
                        select GRANTED_ROLE 
                        from sys.DBA_ROLE_PRIVS 
                        where GRANTEE = SYS_CONTEXT('USERENV','SESSION_USER') and GRANTED_ROLE = 'SINHVIEN'
                    );
    cursor cur_KHMO is  (
                                select distinct MAHP
                                from ADMIN.PROJECT_SINHVIEN SV
                                    inner join ADMIN.PROJECT_KHMO KHMO on SV.MACT = KHMO.MACT
                                where SV.MASV = SYS_CONTEXT('USERENV','SESSION_USER')
                            );
    vaitro varchar(40);
    mahp varchar(5);
    masv varchar2(5);
    STRSQL varchar2(2000);
    counter number;
begin
    open cur;
    fetch cur into vaitro;
    if (cur%FOUND) then
        STRSQL := '(';
        counter := 1;
        for row_KHMO in cur_KHMO
        loop
            if(counter = 1) then
                begin
                    STRSQL := STRSQL || '''' || row_KHMO.MAHP || '''';
                    counter := 2;
                end;
            else
                STRSQL := STRSQL || ',''' || row_KHMO.MAHP || '''';
            end if;
        end loop;
        STRSQL := STRSQL || ')';
        return 'MAHP IN ' || STRSQL;
    else
        return '';
    end if;
end;

grant select on ADMIN.PROJECT_HOCPHAN to SINHVIEN;

begin
    dbms_rls.drop_policy(
        OBJECT_SCHEMA=> 'ADMIN',
        OBJECT_NAME=>'PROJECT_KHMO',
        POLICY_NAME=> 'SINHVIEN_HOCPHAN_SEL'
    );
end;

BEGIN
    dbms_rls.add_policy(
        OBJECT_SCHEMA =>'ADMIN',
        OBJECT_NAME=>'PROJECT_HOCPHAN',
        POLICY_NAME =>'SINHVIEN_HOCPHAN_SEL',
        FUNCTION_SCHEMA => 'sys',
        POLICY_FUNCTION=>'SEC_SINHVIEN_HOCPHAN_SEL',
        STATEMENT_TYPES=>'SELECT'
    );
END;

--connect sys/123@localhost:15211 as SYSDBA;
create or replace function sys.SEC_SINHVIEN_DANGKI_SEL(P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    cursor cur is   (
                        select GRANTED_ROLE 
                        from sys.DBA_ROLE_PRIVS 
                        where GRANTEE = SYS_CONTEXT('USERENV','SESSION_USER') and GRANTED_ROLE = 'SINHVIEN'
                    );
    vaitro varchar(40);
    masv varchar2(5);
begin
    open cur;
    fetch cur into vaitro;
    if (cur%FOUND) then
        masv := SYS_CONTEXT('USERENV','SESSION_USER');
        return 'MASV = ''' || masv || '''';
    else
        return '';
    end if;
end;

grant select on ADMIN.PROJECT_DANGKI to SINHVIEN;

BEGIN
    dbms_rls.add_policy(
        OBJECT_SCHEMA =>'ADMIN',
        OBJECT_NAME=>'PROJECT_DANGKI',
        POLICY_NAME =>'SINHVIEN_DANGKI_SEL',
        FUNCTION_SCHEMA => 'sys',
        POLICY_FUNCTION=>'SEC_SINHVIEN_DANGKI_SEL',
        STATEMENT_TYPES=>'SELECT'
    );
END;

--connect sys/123@localhost:15211 as SYSDBA;
create or replace function sys.SEC_SINHVIEN_DANGKI_INS_DEL(P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    cursor cur is   (
                        select GRANTED_ROLE 
                        from sys.DBA_ROLE_PRIVS 
                        where GRANTEE = SYS_CONTEXT('USERENV','SESSION_USER') and GRANTED_ROLE = 'SINHVIEN'
                    );
    vaitro varchar(40);
    masv varchar2(5);
    ngayHienTai number;
    thangHienTai number;
    namHienTai number;
    hocKiHienTai number;
    ngayBatDauHocKi date;
    ngayBatDauHocKi_str varchar(10);
begin
    open cur;
    fetch cur into vaitro;
    if (cur%FOUND) then
        masv := SYS_CONTEXT('USERENV','SESSION_USER');
        thangHienTai := EXTRACT(month from sysdate);
        namHienTai := EXTRACT(year from sysdate);
        ngayHienTai := EXTRACT(day from sysdate);
        if(thangHienTai >= 1 and thangHienTai < 5) then
            hocKiHienTai := 1;
        elsif (thangHienTai >= 5 and thangHienTai < 9) then
            hocKiHienTai := 5;
        else
            hocKiHienTai := 9;
        end if;
        ngayBatDauHocKi_str := namHienTai || '-' || hocKiHienTai || '-1';
        ngayBatDauHocKi := TO_DATE(ngayBatDauHocKi_str,'YYYY-MM-DD');
        
        
        return 'MASV = ''' || masv || ''' and NAM = ''' || namHienTai || ''' and HK = ''' || hocKiHienTai || ''' and (sysdate - TO_DATE(''' || TO_CHAR(ngayBatDauHocKi) || ''',''DD-MON-YY'')) < 14';
        --return 'MASV = ''' || masv || ''' and NAM = ''' || namHienTai || ''' and HK = ''' || hocKiHienTai || ''' and (TO_DATE(''' || TO_CHAR(ngayBatDauHocKi) || ''',''DD-MON-YY'') - TO_DATE(''' || TO_CHAR(ngayBatDauHocKi) || ''',''DD-MON-YY'')) < 14';
    else
        return '';
    end if;
end;

grant delete on ADMIN.PROJECT_DANGKI to SINHVIEN;
grant insert (MASV,MAGV,MAHP,HK,NAM,MACT) on ADMIN.PROJECT_DANGKI to SINHVIEN;

BEGIN
    dbms_rls.add_policy(
        OBJECT_SCHEMA =>'ADMIN',
        OBJECT_NAME=>'PROJECT_DANGKI',
        POLICY_NAME =>'SINHVIEN_DANGKI_INS_DEL',
        FUNCTION_SCHEMA => 'sys',
        POLICY_FUNCTION=>'SEC_SINHVIEN_DANGKI_INS_DEL',
        STATEMENT_TYPES=>'INSERT,DELETE',
        UPDATE_CHECK => TRUE
    );
END;

declare
    cursor cur_SINHVIEN is (select * from ADMIN.PROJECT_SINHVIEN);
    STRSQL varchar2(1000);
    result boolean;
begin
    for row_SINHVIEN in cur_SINHVIEN
    loop
        result := isUserExists(row_SINHVIEN.MASV);
        if(result = false) then
            STRSQL := 'CREATE USER ' || row_SINHVIEN.MASV || ' identified by 123';
            EXECUTE IMMEDIATE (STRSQL);
        end if;
        STRSQL := 'GRANT SINHVIEN to ' || row_SINHVIEN.MASV;
        EXECUTE IMMEDIATE (STRSQL);
    end loop;
end;
--connect sys/123@localhost:15211 as SYSDBA;
--revoke SINHVIEN from ADMIN CONTAINER = ALL;