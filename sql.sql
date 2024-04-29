create database katsaka;
create table parcelle(
    idparcelle int identity(1,1) primary key,
    name_parcelle varchar(55),
    surface_m2 float
);
create table suivi(
    idsuivi int identity(1,1) primary key, 
    idparcelle int references parcelle(idparcelle),
    nb_tige int ,
    nb_katsaka int ,
    longeure_katsaka float, --cm
    degrecolor float,
    date_suivi date
);
create table tantsaha(
    idtantsaha int identity(1,1) primary key ,
    name_tantsaha varchar(55) 
);
create table tantsaha_post(
    idtantsaha int references tantsaha(idtantsaha),
    idparcelle int references parcelle(idparcelle)
);
create table livraison(
    idlivraison int identity(1,1) primary key ,
    idparcelle int references parcelle(idparcelle),
    nb_maiss int ,
    longeure_katsaka float,
    poidskg float,
    date_livraison date
);
insert into parcelle(name_parcelle , surface_m2) values('parcelle1' , 10.2);
insert into parcelle(name_parcelle , surface_m2) values('parcelle2' , 13.2);
insert into parcelle(name_parcelle , surface_m2) values('parcelle3' , 5.2);


insert into suivi values (1 , 350 , 5 , 10 , 20 , '2023-07-12');
insert into suivi values (2 , 600 , 6 , 11 , 30 , '2023-07-12');
insert into suivi values (3 , 900 , 4 , 12 , 25 , '2023-07-12');

insert into suivi values (1 , 350 , 6 , 15 , 50 , '2023-07-27');
insert into suivi values (2 , 600 , 5 , 12 , 50 , '2023-07-27');
insert into suivi values (3 , 850 , 4 , 17 , 20 , '2023-07-27');

insert into livraison(idparcelle , nb_maiss , longeure_katsaka , poidskg , date_livraison) values (1,2100,15 , 1000 , '2023-08-12');
insert into livraison(idparcelle , nb_maiss , longeure_katsaka , poidskg , date_livraison) values (2,3000,14 , 1200 , '2023-08-12');
insert into livraison(idparcelle , nb_maiss , longeure_katsaka , poidskg , date_livraison) values (3,0,0 ,0 , '2023-08-12');


-- postgres
create database katsaka;
create table parcelle(
    idparcelle serial primary key ,
    name_parcelle varchar,
    surface_m2 float
);
insert into parcelle(name_parcelle , surface_m2) values('parcelle1' , 10.2);
insert into parcelle(name_parcelle , surface_m2) values('parcelle2' , 13.2);
insert into parcelle(name_parcelle , surface_m2) values('parcelle3' , 5.2);

create table zezika(
    idzezika serial primary key ,
    typeZezika varchar
);
insert into zezika (typeZezika) values('A') , ('B'),('C');
create table add_zezika(
    idaddzezika serial primary key ,
    idzezika int references zezika(idzezika),
    idparcelle int references parcelle(idparcelle),
    quantiteKg float ,
    dateajout date
);
-- parcelle A
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(1 , 1 ,0 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(1 , 1 ,10 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(1 , 1 ,100 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(1 , 1 ,10 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(2 , 1 ,100 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(3 , 1 ,100 , '2023-07-12') ;
--parcelle B
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(1 , 2 ,100 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(2 , 2 ,100 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(3 , 2 ,20 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(3 , 2 ,100 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(3 , 2 ,0 , '2023-07-12') ;
--parcelle C
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(1 , 3 ,100 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(2 , 3 ,20 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(2 , 3 ,100 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(3 , 3 ,100 , '2023-07-12') ;
insert into add_zezika (idzezika , idparcelle , quantiteKg , dateajout) values(3 , 3 ,0 , '2023-07-12') ;

-- mysql
create table zezika(
    idzezika int primary key auto_increment ,
    typeZezika varchar(55)
);
create table achatzezika (
    idachat_zezika int primary key auto_increment,
    idzezika int references zezika(idzezika),
    quantiteKg float ,
    prixunit float ,
    dateachat date
);

insert into achatzezika (idzezika , quantiteKg , prixunit , dateachat) values(1 , 1 , 50000 , '2023-07-13');
insert into achatzezika (idzezika , quantiteKg , prixunit , dateachat) values(2 , 1 , 10000 , '2023-07-13');
insert into achatzezika (idzezika , quantiteKg , prixunit , dateachat) values(3 , 1 , 8000 , '2023-07-13');


-- select suivi.idparcelle , suivi.nb_tige , suivi.nb_katsaka , suivi.longeure_katsaka , suivi.date_suivi from  suivi join (select  idparcelle  , max(date_suivi) as date from suivi  group by idparcelle) as date_max on (suivi.idparcelle = date_max.idparcelle and suivi.date_suivi = date_max.date) ; 