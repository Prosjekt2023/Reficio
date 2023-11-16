drop database if exists ReficioDB;
create database if not exists ReficioDB;
use ReficioDB;

-- Create table ServiceFormEntry, if it doesn't exists
create table if not EXISTS ServiceFormEntry
(
      ServiceFormId INT not null unique auto_increment PRIMARY KEY,
      Customer NVARCHAR(255) NOT NULL,
      DateReceived DATE NOT NULL,
      Address NVARCHAR(255),
      Email NVARCHAR(255),
      OrderNumber INT,
      Phone INT,
      ProductType NVARCHAR(255),
      Year INT,
      Service NVARCHAR(255),
      Warranty NVARCHAR(255),
      SerialNumber INT,
      Agreement NVARCHAR(255),
      RepairDescription NVARCHAR(255),
      UsedParts NVARCHAR(255),
      WorkHours NVARCHAR(255),
      CompletionDate NVARCHAR(255),
      ReplacedPartsReturned NVARCHAR(255),
      ShippingMethod NVARCHAR(255),
      CustomerSignature NVARCHAR(255),
      RepairerSignature NVARCHAR(255)
);


create table if not EXISTS AspNetRoles
(
    RolesId varchar(255) not null,
    Name varchar(255),
    NormalizedName  varchar(255),
    ConcurrencyStamp  varchar(255),
    CONSTRAINT U_ROLE_ID_PK PRIMARY KEY (RolesId)
);
-- insert into AspNetRoles(RolesId, Name, NormalizedName) values('Administrator', 'Administrator', 'Administrator');
create table if not EXISTS AspNetUsers
(
         RolesId varchar(255) not null unique,
         UserName varchar(255),
         NormalizedUserName varchar(255),
         Email varchar(255),
         NormalizedEmail varchar(255),
         EmailConfirmed bit not null,
         PasswordHash varchar(255),
         SecurityStamp varchar(255),
         ConcurrencyStamp varchar(255),
         PhoneNumber varchar(50),
         PhoneNumberConfirmed bit not null,
         TwoFactorEnabled bit not null,
         LockoutEnd TIMESTAMP,
         LockoutEnabled bit not null,
         AccessFailedCount int not null,
          CONSTRAINT PK_AspNetUsers PRIMARY KEY (RolesId)
);
create table if not EXISTS AspNetUserTokens
(
    UserId varchar(255) not null,
    LoginProvider varchar(255) not null ,
    Name  varchar(255) not null,
    Value  varchar(255),
    CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId, LoginProvider)
);

create table if not EXISTS AspNetRoleClaims
(
    Id int UNIQUE auto_increment,
    ClaimType varchar(255) not null ,
    ClaimValue  varchar(255) not null,
    RoleId  varchar(255),
    CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
    foreign key(RoleId) 
        references AspNetRoles(RolesId)
); 

create table if not EXISTS AspNetUserClaims
(
    Id int UNIQUE auto_increment,
    ClaimType varchar(255) ,
    ClaimValue  varchar(255),
    UserId  varchar(255),
    CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
    foreign key(UserId) 
        references AspNetUsers(RolesId)
);        

create table if not EXISTS AspNetUserLogins
(
    LoginProvider int UNIQUE auto_increment,
    ProviderKey varchar(255) not null ,
    ProviderDisplayName  varchar(255) not null,
    UserId  varchar(255) not null,
    CONSTRAINT PK_AspNetUserLogins PRIMARY KEY (LoginProvider),
    foreign key(UserId) 
        references AspNetUsers(RolesId)
);       

create table if not EXISTS AspNetUserRoles
(
    UserId varchar(255) not null,
    RoleId varchar(255) not null,
    CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId,RoleId),
    foreign key(UserId) 
        references AspNetUsers(RolesId),
    foreign key(RoleId) 
        references AspNetRoles(RolesId)
);


-- Table for the checklist
CREATE TABLE IF NOT EXISTS Checklist
(
    ChecklistId INT AUTO_INCREMENT PRIMARY KEY,
    Sign VARCHAR(255), -- Signature
    Freeform TEXT, -- Any additional freeform text or comments
    CompletionDate DATE NOT NULL -- The date the checklist was completed
);

-- Table for the checkpoints
CREATE TABLE IF NOT EXISTS CheckpointsEntry
(
    CheckpointId INT AUTO_INCREMENT PRIMARY KEY,
    ChecklistId INT UNIQUE, -- This ensures that each CheckpointsEntry can only be associated with one Checklist
    ClutchCheck VARCHAR(50),
    BrakeCheck VARCHAR(50),
    DrumBearingCheck VARCHAR(50),
    PTOCheck VARCHAR(50),
    ChainTensionCheck VARCHAR(50),
    WireCheck VARCHAR(50),
    PinionBearingCheck VARCHAR(50),
    ChainWheelKeyCheck VARCHAR(50),
    HydraulicCylinderCheck VARCHAR(50),
    HoseCheck VARCHAR(50),
    HydraulicBlockTest VARCHAR(50),
    TankOilChange VARCHAR(50),
    GearboxOilChange VARCHAR(50),
    RingCylinderSealsCheck VARCHAR(50),
    BrakeCylinderSealsCheck VARCHAR(50),
    WinchWiringCheck VARCHAR(50),
    RadioCheck VARCHAR(50),
    ButtonBoxCheck VARCHAR(50),
    PressureSettings VARCHAR(50),
    FunctionTest VARCHAR(50),
    TractionForceKN VARCHAR(50),
    BrakeForceKN VARCHAR(50),
    FOREIGN KEY (ChecklistId) REFERENCES Checklist (ChecklistId)
);

-- Tabel-for-userAccount 
CREATE TABLE IF NOT EXISTS userAccount (
    userID INT PRIMARY KEY auto_increment,
    password VARCHAR(50) not null,
    loginStatus bool not null, -- Gjorde om til bool type
    fullName VARCHAR(100) not null,
    address VARCHAR(100) not null,
    email VARCHAR(50) not null
);

-- Tabel-for-Mekaniker
CREATE table if not exists mekaniker (
    userID int,
    FOREIGN KEY (userID) references userAccount (userID)
);

-- Tabel-for-Service_ansatt
CREATE TABLE IF NOT EXISTS service_ansatt (
    userID int,
    FOREIGN KEY (userID) references userAccount (userID)
);