CREATE TABLE IF NOT EXISTS Customer
(
    CustomerId INT NOT NULL UNIQUE AUTO_INCREMENT PRIMARY KEY,
    CustomerName NVARCHAR(255) NOT NULL,
    Address NVARCHAR(255),
    Email NVARCHAR(255),
    Phone NVARCHAR(255)
);

CREATE TABLE IF NOT EXISTS ProductInfo
(
    ProductId INT NOT NULL UNIQUE AUTO_INCREMENT PRIMARY KEY,
    ProductType NVARCHAR(255),
    Year INT,
    Service NVARCHAR(255),
    Warranty NVARCHAR(255),
    SerialNumber NVARCHAR(255)
);

CREATE TABLE IF NOT EXISTS PartsInfo
(
    PartsId INT NOT NULL UNIQUE AUTO_INCREMENT PRIMARY KEY,
    UsedParts NVARCHAR(255),
    ReplacedPartsReturned NVARCHAR(255)
);

CREATE TABLE IF NOT EXISTS Signatures
(
    SignatureId INT NOT NULL UNIQUE AUTO_INCREMENT PRIMARY KEY,
    CustomerSignature NVARCHAR(255),
    RepairerSignature NVARCHAR(255)
);

CREATE TABLE IF NOT EXISTS ServiceDates
(
    DateId INT NOT NULL UNIQUE AUTO_INCREMENT PRIMARY KEY,
    DateReceived DATE NOT NULL,
    CompletionDate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS ServiceFormEntry
(
    ServiceFormId INT NOT NULL UNIQUE AUTO_INCREMENT PRIMARY KEY,
    CustomerId INT,
    ProductId INT,
    PartsId INT,
    SignatureId INT,
    DateId INT,
    OrderNumber INT,
    Agreement NVARCHAR(255),
    RepairDescription NVARCHAR(255),
    WorkHours NVARCHAR(255),
    ShippingMethod NVARCHAR(255),
    FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId),
    FOREIGN KEY (ProductId) REFERENCES ProductInfo(ProductId),
    FOREIGN KEY (PartsId) REFERENCES PartsInfo(PartsId),
    FOREIGN KEY (SignatureId) REFERENCES Signatures(SignatureId),
    FOREIGN KEY (DateId) REFERENCES ServiceDates(DateId)
);

CREATE TABLE IF NOT EXISTS Checklist
(
    ChecklistId INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    MechanicalCheckId INT,
    HydraulicCheckId INT,
    ElectricalCheckId INT,
    GeneralCheckId INT,
    Sign VARCHAR(255),
    Freeform TEXT,
    CompletionDate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS MechanicalChecks
(
    MechanicalCheckId INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    ChecklistId INT,
    ClutchCheck VARCHAR(50),
    BrakeCheck VARCHAR(50),
    DrumBearingCheck VARCHAR(50),
    PTOCheck VARCHAR(50),
    ChainTensionCheck VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS HydraulicChecks
(
    HydraulicCheckId INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    ChecklistId INT,
    HydraulicCylinderCheck VARCHAR(50),
    HoseCheck VARCHAR(50),
    HydraulicBlockTest VARCHAR(50),
    TankOilChange VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS ElectricalChecks
(
    ElectricalCheckId INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    ChecklistId INT,
    WinchWiringCheck VARCHAR(50),
    RadioCheck VARCHAR(50),
    ButtonBoxCheck VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS GeneralChecks
(
    GeneralCheckId INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    ChecklistId INT,
    PressureSettings VARCHAR(50),
    FunctionTest VARCHAR(50),
    TractionForceKN VARCHAR(50),
    BrakeForceKN VARCHAR(50)
);

ALTER TABLE Checklist
    ADD FOREIGN KEY (MechanicalCheckId) REFERENCES MechanicalChecks(MechanicalCheckId),
    ADD FOREIGN KEY (HydraulicCheckId) REFERENCES HydraulicChecks(HydraulicCheckId),
    ADD FOREIGN KEY (ElectricalCheckId) REFERENCES ElectricalChecks(ElectricalCheckId),
    ADD FOREIGN KEY (GeneralCheckId) REFERENCES GeneralChecks(GeneralCheckId);

ALTER TABLE MechanicalChecks
    ADD FOREIGN KEY (ChecklistId) REFERENCES Checklist(ChecklistId);

ALTER TABLE HydraulicChecks
    ADD FOREIGN KEY (ChecklistId) REFERENCES Checklist(ChecklistId);

ALTER TABLE ElectricalChecks
    ADD FOREIGN KEY (ChecklistId) REFERENCES Checklist(ChecklistId);

ALTER TABLE GeneralChecks
    ADD FOREIGN KEY (ChecklistId) REFERENCES Checklist(ChecklistId);
