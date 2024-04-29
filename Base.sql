CREATE DATABASE katsaka;
\c katsaka;

CREATE SEQUENCE seq_terrain;
CREATE SEQUENCE seq_parcelle;
CREATE SEQUENCE seq_mpamboly;
CREATE SEQUENCE seq_responsable;
CREATE SEQUENCE seq_culture;
CREATE SEQUENCE seq_suivi;
CREATE SEQUENCE seq_recolte;

CREATE TABLE terrain (
    idterrain VARCHAR(15) DEFAULT 'TER'||nextval('seq_terrain') PRIMARY KEY,
    mesure FLOAT
);
INSERT INTO terrain(mesure) VALUES(10000);

CREATE TABLE parcelle (
    idparcelle VARCHAR(15) DEFAULT 'PAR'||nextval('seq_parcelle') PRIMARY KEY,
    nomparcelle VARCHAR(30),
    idterrain VARCHAR(15),
    mesure FLOAT,
    FOREIGN KEY (idterrain) REFERENCES terrain(idterrain)
);
INSERT INTO parcelle(nomparcelle,idterrain,mesure) VALUES('Parcelle1','TER1',3000);
INSERT INTO parcelle(nomparcelle,idterrain,mesure) VALUES('Parcelle2','TER1',2000);
INSERT INTO parcelle(nomparcelle,idterrain,mesure) VALUES('Parcelle3','TER1',4000);

CREATE TABLE mpamboly (
    idmpamboly VARCHAR(15) DEFAULT 'MPA'||nextval('seq_mpamboly') PRIMARY KEY,
    nommpamboly VARCHAR(30)
);
INSERT INTO mpamboly(nommpamboly) VALUES('Rakoto');
INSERT INTO mpamboly(nommpamboly) VALUES('Jean');

CREATE TABLE responsable (
    idresponsable VARCHAR(15) DEFAULT 'RES'||nextval('seq_responsable') PRIMARY KEY,
    idmpamboly VARCHAR(15),
    idparcelle VARCHAR(15),
    FOREIGN KEY (idparcelle) REFERENCES parcelle(idparcelle),
    FOREIGN KEY (idmpamboly) REFERENCES mpamboly(idmpamboly)
);
INSERT INTO responsable(idmpamboly,idparcelle) VALUES('MPA1','PAR1');
INSERT INTO responsable(idmpamboly,idparcelle) VALUES('MPA1','PAR2');
INSERT INTO responsable(idmpamboly,idparcelle) VALUES('MPA2','PAR3');

CREATE TABLE culture (
    idculture VARCHAR(15) DEFAULT 'CUL'||nextval('seq_culture') PRIMARY KEY,
    idparcelle VARCHAR(15),
    nbrtahony FLOAT,    
    dateculture TIMESTAMP,
    etat int,
    FOREIGN KEY (idparcelle) REFERENCES parcelle(idparcelle)
);

CREATE TABLE suivi (
    idsuivi VARCHAR(15) DEFAULT 'SUI'||nextval('seq_suivi') PRIMARY KEY,
    idresponsable VARCHAR(15),
    datesuivi DATE,
    nbrtahony FLOAT,
    nbrtolany FLOAT,
    longueurtolany FLOAT,
    nivverrete INT,
    croissance FLOAT,
    semaine INT,
    FOREIGN KEY (idresponsable) REFERENCES responsable(idresponsable)
);

CREATE TABLE recolte (
    idrecolte VARCHAR(15) DEFAULT 'REC'||nextval('seq_recolte') PRIMARY KEY,
    idresponsable VARCHAR(15),
    nbrtolany FLOAT, 
    longueurtolany FLOAT,
    poidsrecolte FLOAT,
    daterecolte DATE,
    FOREIGN KEY (idresponsable) REFERENCES responsable(idresponsable)
);

CREATE VIEW champ AS SELECT p.*,res.idresponsable,res.idmpamboly FROM parcelle as p JOIN
responsable as res ON p.idparcelle = res.idparcelle;
 
CREATE VIEW suiviparcelle AS SELECT s.*,res.idmpamboly,res.idparcelle FROM suivi as s JOIN responsable as res on s.idresponsable = res.idresponsable; 


CREATE SEQUENCE seq_typeanomalie;
CREATE SEQUENCE seq_anomalie;
CREATE SEQUENCE seq_anomalierecolte;

CREATE TABLE typeanomalie (
    idtypeanomalie VARCHAR(15) DEFAULT 'TYA'||nextval('seq_typeanomalie') PRIMARY KEY,
    motif VARCHAR(50)
);
INSERT INTO typeanomalie(motif) VALUES('Le nombre du tige de mais a diminue');
INSERT INTO typeanomalie(motif) VALUES('Le nombre moyen de mais par tige a diminue');
INSERT INTO typeanomalie(motif) VALUES('La longueur du mais a diminue');
INSERT INTO typeanomalie(motif) VALUES('Le mais manque d eau, sa couleur jaunissent');

CREATE TABLE anomalie (
    idanomalie VARCHAR(15) DEFAULT 'ANO'||nextval('seq_anomalie') PRIMARY KEY,
    idsuivi VARCHAR(15),
    idtypeanomalie VARCHAR(15),
    FOREIGN KEY (idsuivi) REFERENCES suivi(idsuivi),
    FOREIGN KEY (idtypeanomalie) REFERENCES typeanomalie(idtypeanomalie)
);

CREATE TABLE anomalierecolte (
    idanomalie VARCHAR(15) DEFAULT 'ANO'||nextval('seq_anomalierecolte') PRIMARY KEY,
    idrecolte VARCHAR(15),
    idtypeanomalie VARCHAR(15),
    FOREIGN KEY (idrecolte) REFERENCES recolte(idrecolte),
    FOREIGN KEY (idtypeanomalie) REFERENCES typeanomalie(idtypeanomalie)
);

CREATE VIEW detailsano as
SELECT ano.*,typ.motif FROM anomalie as ano
JOIN typeanomalie as typ ON ano.idtypeanomalie = typ.idtypeanomalie;

CREATE VIEW detailsanoreco as
SELECT ano.*,typ.motif FROM anomalierecolte as ano
JOIN typeanomalie as typ ON ano.idtypeanomalie = typ.idtypeanomalie;

CREATE VIEW anosuivi as
SELECT ano.*,s.idresponsable,s.semaine,s.datesuivi FROM anomalie AS ano
JOIN suivi as s ON ano.idsuivi = s.idsuivi;



/*********************** MySql ***********************/
CREATE DATABASE katsaka;
USE katsaka;

CREATE TABLE zezika (
    idzezika INT PRIMARY KEY auto_increment,
    nomzezika VARCHAR(25),
    prixunitaire FLOAT
);

CREATE TABLE additif(
    idadditif INT PRIMARY KEY auto_increment,
    idzezika int,
    percentmin FLOAT,
    percentmax FLOAT,
    variation FLOAT,
    FOREIGN KEY (idzezika) REFERENCES zezika(idzezika)
);

INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(1,0,25,0);
INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(1,26,50,-5);
INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(1,51,100,10);

INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(2,0,25,10);
INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(2,26,50,-5);
INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(2,51,100,-30);

INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(3,0,25,-10);
INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(3,26,50,5);
INSERT INTO additif(idzezika,percentmin,percentmax,variation) VALUES(3,51,100,0);

CREATE TABLE ajoutadditif(
    idajoutadditif INT PRIMARY KEY auto_increment,
    idzezika int,
    idparcelle VARCHAR(25)
);

INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(1,'PAR1');
INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(2,'PAR1');
INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(3,'PAR1');

INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(1,'PAR2');
INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(2,'PAR2');
INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(3,'PAR2');

INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(1,'PAR3');
INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(2,'PAR3');
INSERT INTO ajoutadditif(idzezika,idparcelle) VALUES(3,'PAR3');

CREATE VIEW detailsadditif AS
SELECT aj.*,ad.percentmin,ad.percentmax,ad.variation FROM ajoutadditif as aj
JOIN additif as ad ON aj.idzezika=ad.idzezika;

INSERT INTO zezika(nomzezika,prixunitaire) VALUES("ZezikaA",500);
INSERT INTO zezika(nomzezika,prixunitaire) VALUES("ZezikaB",450);
INSERT INTO zezika(nomzezika,prixunitaire) VALUES("ZezikaC",520);

insert into recolte(idresponsable,nbrtolany,longueurtolany,poidsrecolte,daterecolte) VALUES('RES1',2100,15,1000,'2023-07-01');
insert into recolte(idresponsable,nbrtolany,longueurtolany,poidsrecolte,daterecolte) VALUES('RES2',3000,15,1142.25,'2023-07-01');
insert into recolte(idresponsable,nbrtolany,longueurtolany,poidsrecolte,daterecolte) VALUES('RES3',3400,15,1834.92,'2023-07-01');