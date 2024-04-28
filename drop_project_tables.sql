drop table ADMIN.PROJECT_DANGKI;
drop table ADMIN.PROJECT_PHANCONG;
drop table ADMIN.PROJECT_SINHVIEN;
drop table ADMIN.PROJECT_NHANSU;
drop table ADMIN.PROJECT_KHMO;
drop table ADMIN.PROJECT_HOCPHAN;
drop table ADMIN.PROJECT_DONVI;

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

declare
    cursor cur_NHANSU is (select * from ADMIN.PROJECT_NHANSU);
    STRSQL varchar2(1000);
    result boolean;
begin
    for row_NHANSU in cur_NHANSU
    loop
        result := isUserExists(row_NHANSU.MANV);
        if(result = TRUE) then
            STRSQL := 'DROP USER ' || row_NHANSU.MANV;
            EXECUTE IMMEDIATE (STRSQL);
        end if;
    end loop;
end;

declare
    cursor cur_SINHVIEN is (select * from ADMIN.PROJECT_SINHVIEN);
    STRSQL varchar2(1000);
    result boolean;
begin
    for row_SINHVIEN in cur_SINHVIEN
    loop
        result := isUserExists(row_SINHVIEN.MANV);
        if(result = TRUE) then
            STRSQL := 'DROP USER ' || row_SINHVIEN.MANV;
            EXECUTE IMMEDIATE (STRSQL);
        end if;
    end loop;
end;