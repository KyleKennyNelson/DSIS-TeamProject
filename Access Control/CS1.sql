--drop role NVCOBAN;
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
create role NVCOBAN;
ALTER SESSION SET "_ORACLE_SCRIPT" = FALSE;

grant connect to NVCOBAN;
grant select on ADMIN.PROJECT_SINHVIEN to NVCOBAN;
grant select on ADMIN.PROJECT_DONVI to NVCOBAN;
grant select on ADMIN.PROJECT_HOCPHAN to NVCOBAN;
grant select on ADMIN.PROJECT_KHMO to NVCOBAN;

CREATE OR REPLACE VIEW admin.UV_TTCANHAN_NHANSU
AS 
    SELECT  NV.MANV , NV.HOTEN, NV.PHAI, NV.NGSINH, NV.PHUCAP, NV.DT, NV.VAITRO, NV.DONVI
    FROM  PROJECT_NHANSU  NV
    WHERE NV.MANV = SYS_CONTEXT('USERENV','SESSION_USER');
    
    
CREATE OR REPLACE VIEW admin.UV_SDT_NHANSU
AS 
    SELECT  NV.DT
    FROM  PROJECT_NHANSU  NV
    WHERE NV.MANV = SYS_CONTEXT('USERENV','SESSION_USER');
    
grant select on ADMIN.UV_TTCANHAN_NHANSU to NVCOBAN;
grant update on ADMIN.UV_SDT_NHANSU to NVCOBAN;

declare
    cursor cur_NVCOBAN is (select * from ADMIN.PROJECT_NHANSU where VAITRO = 'NVCOBAN');
    STRSQL varchar2(1000);
begin
    for row_NVCOBAN in cur_NVCOBAN
    loop
        STRSQL := 'CREATE USER ' || row_NVCOBAN.MANV || ' identified by 123';
        EXECUTE IMMEDIATE (STRSQL);
        STRSQL := 'GRANT NVCOBAN to ' || row_NVCOBAN.MANV;
        EXECUTE IMMEDIATE (STRSQL);
    end loop;
end;