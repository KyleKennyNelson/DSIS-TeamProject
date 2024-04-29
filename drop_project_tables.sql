drop table ADMIN.PROJECT_DANGKI;
drop table ADMIN.PROJECT_PHANCONG;
drop table ADMIN.PROJECT_SINHVIEN;
drop table ADMIN.PROJECT_NHANSU;
drop table ADMIN.PROJECT_KHMO;
drop table ADMIN.PROJECT_HOCPHAN;
drop table ADMIN.PROJECT_DONVI;

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