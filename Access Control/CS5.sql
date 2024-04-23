ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
create role TRUONGKHOA;
--drop role TRUONGKHOA;
ALTER SESSION SET "_ORACLE_SCRIPT" = FALSE;

--NHU MOT NGUOI DUNG CO VAI TRO GIANG VIEN
grant GIANGVIEN to TRUONGKHOA;
grant connect to TRUONGKHOA;

--TAO VIEW TREN QUAN HE PHAN CONG DOI VOI CAC HOC PHAN QUAN LY BOI DON VI VAN PHONG KHOA
create or replace view ADMIN.UV_TRUONGKHOA_PHANCONG
as
    select PC.*
    from ADMIN.PROJECT_PHANCONG PC, ADMIN.PROJECT_HOCPHAN HP, ADMIN.PROJECT_DONVI DV
    where PC.MAHP = HP.MAHP
        AND HP.MADV = DV.MADV
        AND dv.tendv = 'VP KHOA';
        
--THEM, XOA, CAP, NHAT ADMIN.UV_TRUONGKHOA_PHANCONG
grant SELECT, INSERT, DELETE, UPDATE on ADMIN.UV_TRUONGKHOA_PHANCONG to TRUONGKHOA;

--XEM, THEM, XOA, SUA, TREN BANG NHAN SU
grant SELECT, INSERT, DELETE, UPDATE on ADMIN.PROJECT_NHANSU to TRUONGKHOA;

--XEM TAT CA CAC BANG
declare
    cursor c1 is select table_name from user_tables;
    cmd varchar2(200);
begin
    for c in c1 
    loop
        cmd := 'GRANT SELECT ON '||c.table_name|| ' TO TRUONGKHOA';
        execute immediate cmd;
    end loop;
end;