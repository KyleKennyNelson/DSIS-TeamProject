--Tao role nhan vien giao vu
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
create role GIAOVU;
ALTER SESSION SET "_ORACLE_SCRIPT" = FALSE;


--Quyen nhan vien co ban
grant NVCOBAN to GIAOVU;

--Quyen insert va quyen update tren cac quan he sau
grant select,insert, update on ADMIN.PROJECT_SINHVIEN to GIAOVU;
grant select,insert, update on ADMIN.PROJECT_DONVI to GIAOVU;
grant select,insert, update on ADMIN.PROJECT_HOCPHAN to GIAOVU;
grant select,insert, update on ADMIN.PROJECT_KHMO to GIAOVU;

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

create or replace function sys.SEC_GIAOVU_PHANCONG_DEL(P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    cursor cur is   (
                        select GRANTED_ROLE 
                        from sys.DBA_ROLE_PRIVS 
                        where GRANTEE = SYS_CONTEXT('USERENV','SESSION_USER') and GRANTED_ROLE = 'GIAOVU'
                    );
    cursor cur_HOCPHAN is  (
                                SELECT  HP.*
                                FROM  ADMIN.PROJECT_DONVI  DV, ADMIN.PROJECT_HOCPHAN HP
                                WHERE dv.tendv = 'VP KHOA' AND dv.madv = HP.MADV
                            );
    vaitro varchar(40);
    STRSQL varchar2(2000);
begin
    open cur;
    fetch cur into vaitro;
    if (cur%FOUND) then
        begin
            STRSQL := 'MAHP in (';
            for row_HOCPHAN in cur_HOCPHAN
            loop
                STRSQL := STRSQL || '''' || row_HOCPHAN.MAHP || ''', ';                     
            end loop;
            STRSQL := STRSQL || ''''')';
            return STRSQL;
        end;
    else
        return '';
    end if;
end;

grant delete on ADMIN.PROJECT_PHANCONG to GIAOVU;

BEGIN
    dbms_rls.add_policy(
        OBJECT_SCHEMA =>'ADMIN',
        OBJECT_NAME=>'PROJECT_PHANCONG',
        POLICY_NAME =>'SGIAOVU_PHANCONG_DEL',
        FUNCTION_SCHEMA => 'sys',
        POLICY_FUNCTION=>'SEC_GIAOVU_PHANCONG_DEL',
        STATEMENT_TYPES=>'DELETE'
    );
END;

--VIEW CHINH SUA QUAN H? DANGKY
--CREATE OR REPLACE VIEW admin.UV_GV_DANGKI
--AS 
    --SELECT  *
    --FROM  PROJECT_DANGKI DK
    --WHERE SYSDATE <= TO_DATE(DK.NAM ||'-' || DK.HK || '-15', 'YYYY-MM-DD') ;


--GRANT QUYEN DELETE V� INSERT TREN QUAN HE DANG KI
--GRANT DELETE,INSERT ON ADMIN.UV_GV_DANGKI TO GIAOVU;

create or replace function sys.SEC_GIAOVU_DANGKI_INS_DEL(P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    cursor cur is   (
                        select GRANTED_ROLE 
                        from sys.DBA_ROLE_PRIVS 
                        where GRANTEE = SYS_CONTEXT('USERENV','SESSION_USER') and GRANTED_ROLE = 'GIAOVU'
                    );
    vaitro varchar(40);
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
        
        
        return 'NAM = ''' || namHienTai || ''' and HK = ''' || hocKiHienTai || ''' and (sysdate - TO_DATE(''' || TO_CHAR(ngayBatDauHocKi) || ''',''DD-MON-YY'')) < 14';
        --return 'NAM = ''' || namHienTai || ''' and HK = ''' || hocKiHienTai || ''' and (TO_DATE(''' || TO_CHAR(ngayBatDauHocKi) || ''',''DD-MON-YY'') - TO_DATE(''' || TO_CHAR(ngayBatDauHocKi) || ''',''DD-MON-YY'')) < 14';
    else
        return '';
    end if;
end;

grant select, delete on ADMIN.PROJECT_DANGKI to GIAOVU;
grant insert (MASV,MAGV,MAHP,HK,NAM,MACT) on ADMIN.PROJECT_DANGKI to GIAOVU;

BEGIN
    dbms_rls.add_policy(
        OBJECT_SCHEMA =>'ADMIN',
        OBJECT_NAME=>'PROJECT_DANGKI',
        POLICY_NAME =>'GIAOVU_DANGKI_INS_DEL',
        FUNCTION_SCHEMA => 'sys',
        POLICY_FUNCTION=>'SEC_GIAOVU_DANGKI_INS_DEL',
        STATEMENT_TYPES=>'INSERT,DELETE',
        UPDATE_CHECK => TRUE
    );
END;

declare
    cursor cur_GIAOVU is (select * from ADMIN.PROJECT_NHANSU where VAITRO = 'GIAOVU');
    STRSQL varchar2(1000);
    result boolean;
begin
    for row_GIAOVU in cur_GIAOVU
    loop
        result := ADMIN.isUserExists(row_GIAOVU.MANV);
        if(result = false) then
            STRSQL := 'CREATE USER ' || row_GIAOVU.MANV || ' identified by 123';
            EXECUTE IMMEDIATE (STRSQL);
            STRSQL := 'GRANT GIAOVU to ' || row_GIAOVU.MANV;
            EXECUTE IMMEDIATE (STRSQL);
        end if;
    end loop;
end;