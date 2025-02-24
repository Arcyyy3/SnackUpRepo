use [DB_SnackUpProject]

-- Creazione della tabella Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(100) NOT NULL,
    Surname VARCHAR(100) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Verified int NOT NULL,
    VerificationToken VARCHAR(500) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
    SchoolClassID INT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL
	);
-- Creazione della tabella Schools
	CREATE TABLE Schools (
		SchoolID INT PRIMARY KEY IDENTITY(1,1),
		SchoolName VARCHAR(255) NOT NULL,
		Address VARCHAR(255) NOT NULL,
		ProducerID INT NULL,
		Created DATETIME NOT NULL DEFAULT GETDATE(),
		Modified DATETIME NULL,
		Deleted DATETIME NULL
	);
    
CREATE TABLE SchoolClasses (
    SchoolClassID INT PRIMARY KEY IDENTITY(1,1),
    SchoolID INT NOT NULL,
    ClassYear INT NOT NULL,
    ClassSection VARCHAR(10) NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_SchoolClasses_Schools FOREIGN KEY (SchoolID) REFERENCES Schools(SchoolID) ON DELETE CASCADE
);

ALTER TABLE Users
ADD CONSTRAINT FK_Users_SchoolClasses FOREIGN KEY (SchoolClassID) REFERENCES SchoolClasses(SchoolClassID) ON DELETE SET NULL;

-- Creazione della tabella Producers
CREATE TABLE Producers (
    ProducerID INT PRIMARY KEY IDENTITY(1,1),
    ProducerName VARCHAR(255) NOT NULL,
    Address VARCHAR(255),
    ContactInfo VARCHAR(255),
	PhotoLinkProduttore VARCHAR(500),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL
);

-- Creazione della tabella Products
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
	BundleID INT ,
    ProductName VARCHAR(255) NOT NULL,
    Description TEXT,
	Details TEXT,
	Raccomandation TEXT,
    Price decimal ,
	PhotoLinkProdotto VARCHAR(500),
    ProducerID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_Products_Producers FOREIGN KEY (ProducerID) REFERENCES Producers(ProducerID) ON DELETE CASCADE
);

-- Creazione della tabella Promotions
CREATE TABLE Promotions (
    PromotionID INT PRIMARY KEY IDENTITY(1,1),
    PromotionName VARCHAR(255) NOT NULL,
    Description TEXT,
    DiscountPercentage decimal
	(5,2) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL
);

-- Creazione della tabella ProductPromotions (relazione molti-a-molti)
CREATE TABLE ProductPromotions (
    ProductPromotionID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    PromotionID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_ProductPromotions_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    CONSTRAINT FK_ProductPromotions_Promotions FOREIGN KEY (PromotionID) REFERENCES Promotions(PromotionID) ON DELETE CASCADE
);

	-- Creazione della tabella Orders
	CREATE TABLE Orders (
		OrderID INT PRIMARY KEY IDENTITY,
		UserID INT NOT NULL,
		SchoolClassID INT NOT NULL,
		Status NVARCHAR(50) NOT NULL,
		TotalPrice DECIMAL(10, 2),
		Created DATETIME DEFAULT GETDATE(),
		Modified DATETIME NULL,
		Deleted DATETIME NULL,
		CONSTRAINT FK_Orders_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
		 CONSTRAINT FK_Orders_SchoolClasses FOREIGN KEY (SchoolClassID) REFERENCES SchoolClasses(SchoolClassID) ON DELETE CASCADE
	);


CREATE TABLE OrderDetails (
    DetailID INT PRIMARY KEY IDENTITY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    DeliveryDate DATE NOT NULL, -- Data di consegna specifica
    Recreation NVARCHAR(10) NOT NULL, -- "First" o "Second"
	ProductCode NVARCHAR(10) NOT NULL,
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);





-- Creazione della tabella OrderTrackings
CREATE TABLE OrderTrackings (
    OrderTrackingID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    Status VARCHAR(50) NOT NULL,
    LastUpdate DATETIME NOT NULL DEFAULT GETDATE(),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_OrderTrackings_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE
);

-- Creazione della tabella Payments
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NULL,
    SubscriptionID INT NULL,
    Amount decimal(10,2) NOT NULL,
    PaymentMethod VARCHAR(50) NOT NULL,
    PaymentDate DATETIME NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE SET NULL
);

-- Creazione della tabella RefreshTokens
CREATE TABLE RefreshTokens (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    Token VARCHAR(255) NOT NULL,
    ExpirationDate DATETIME NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsRevoked BIT NOT NULL DEFAULT 0,
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- Creazione della tabella SupportRequests
-- Creazione della tabella SupportRequests
CREATE TABLE SupportRequests (
    SupportRequestID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    OrderID INT NULL, -- Può essere NULL perché non tutte le richieste riguardano ordini
    Subject VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Status VARCHAR(50) NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_SupportRequests_Users FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    CONSTRAINT FK_SupportRequests_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE NO ACTION
);
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(255) NOT NULL UNIQUE,
    Description NVARCHAR(500) NULL,
    Created DATETIME DEFAULT GETUTCDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL
);
CREATE TABLE CategoryProducts (
    ProductID INT NOT NULL,
    CategoryID INT NOT NULL,
    Created DATETIME DEFAULT GETUTCDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);
CREATE TABLE Inventory (
    ProductID INT PRIMARY KEY, -- Chiave primaria
    QuantityAvailable INT NOT NULL, -- Scorte disponibili
    QuantityReserved INT DEFAULT 0, -- Quantità riservata
    ReorderLevel INT DEFAULT 10, -- Livello di riordino
    LastUpdated DATETIME NOT NULL DEFAULT GETDATE(), -- Ultima modifica
    Created DATETIME NOT NULL DEFAULT GETUTCDATE(), -- Data di creazione
    Modified DATETIME NULL, -- Data di modifica
    Deleted DATETIME NULL, -- Soft delete
    CONSTRAINT FK_Inventory_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE -- Relazione con Products
);


-- Tabella InventoryHistory
CREATE TABLE InventoryHistory (
    HistoryID INT PRIMARY KEY IDENTITY(1,1), -- Chiave primaria autonumerata
    ProductID INT NOT NULL, -- Prodotto a cui si riferisce
    ChangeAmount INT NOT NULL, -- Quantità aggiunta o rimossa (+ o -)
    ChangeReason NVARCHAR(255) NOT NULL, -- Motivo del cambiamento ("Order", "Manual Update", etc.)
    ChangedBy NVARCHAR(100) NOT NULL, -- Utente che ha effettuato il cambiamento
    Created DATETIME NOT NULL DEFAULT GETUTCDATE(), -- Data di creazione
    Modified DATETIME NULL, -- Data di modifica
    Deleted DATETIME NULL, -- Soft delete
    ChangeDate DATETIME NOT NULL DEFAULT GETDATE(), -- Data della modifica
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE -- Relazione con Products
);
CREATE TABLE ShoppingSessions (
    SessionID INT PRIMARY KEY IDENTITY(1,1), -- ID univoco della sessione
    UserID INT NULL, -- Collegamento all'utente, NULL se anonimo
    Status VARCHAR(20) NOT NULL DEFAULT 'Active', -- Es.: Active, Completed, Cancelled
    TotalAmount decimal(10,2) NOT NULL DEFAULT 0.00, -- Totale della sessione
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), -- Data di creazione
    UpdatedAt DATETIME NULL, -- Data di aggiornamento
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE SET NULL -- Collegamento alla tabella Users
);
CREATE TABLE CartItems (
    CartItemID INT PRIMARY KEY IDENTITY(1,1), -- ID univoco per l'elemento del carrello
    SessionID INT NOT NULL, -- Collegamento alla sessione
    ProductID INT NOT NULL, -- Collegamento al prodotto
    Quantity INT NOT NULL DEFAULT 1, -- Quantità del prodotto
    Price decimal(10,2) NOT NULL, -- Prezzo unitario del prodotto al momento dell'aggiunta
    Total decimal(10,2) NOT NULL, -- Totale per questa voce (Price * Quantity)
	DeliveryDate DATETIME, -- Data di creazione
	Recreation nVarchar(50),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), -- Data di creazione
    UpdatedAt DATETIME NULL, -- Data di aggiornamento
	DeletedAt DATETIME NULL, -- Data di aggiornamento
    FOREIGN KEY (SessionID) REFERENCES ShoppingSessions(SessionID) ON DELETE CASCADE, -- Collegamento alla sessione
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE -- Collegamento alla tabella Products
);
CREATE TABLE ClassDeliveryCodes (
    ClassDeliveryCodeID INT PRIMARY KEY IDENTITY(1,1),
    SchoolClassID INT NOT NULL, -- Collegamento alla classe
    Code1 VARCHAR(10) NOT NULL, -- Codice per la prima ricreazione
    Code2 VARCHAR(10) NOT NULL, -- Codice per la seconda ricreazione
    RetrievedCode1 BIT DEFAULT 0, -- Stato di conferma ritiro prima ricreazione
    RetrievedCode2 BIT DEFAULT 0, -- Stato di conferma ritiro seconda ricreazione
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_ClassDeliveryCodes_SchoolClasses FOREIGN KEY (SchoolClassID) REFERENCES SchoolClasses(SchoolClassID) ON DELETE CASCADE
);
CREATE TABLE ClassDeliveryLogs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    ClassDeliveryCodeID INT NOT NULL, -- Collegamento ai codici della classe
    UserID INT NOT NULL, -- `MasterStudent` che ha confermato
    CodeType VARCHAR(10) NOT NULL, -- "Code1" o "Code2"
    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (ClassDeliveryCodeID) REFERENCES ClassDeliveryCodes(ClassDeliveryCodeID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
-- Tabella degli allergeni
CREATE TABLE Allergens (
    AllergenID INT PRIMARY KEY IDENTITY(1,1),
    AllergenName NVARCHAR(255) NOT NULL UNIQUE,
    Description NVARCHAR(500) NULL,
    Created DATETIME DEFAULT GETUTCDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL
);

-- Relazione molti-a-molti tra prodotti e allergeni
CREATE TABLE ProductAllergens (
    ProductAllergenID INT IDENTITY(1,1),
    ProductID INT NOT NULL,
    AllergenID INT NOT NULL,
    Created DATETIME DEFAULT GETUTCDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    PRIMARY KEY (ProductID, AllergenID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    FOREIGN KEY (AllergenID) REFERENCES Allergens(AllergenID) ON DELETE CASCADE
);
-- Relazione molti-a-molti tra prodotti e allergeni
CREATE TABLE ProducerUsers (
    ProducerID INT NOT NULL,
    UserID INT NOT NULL,
    Created DATETIME DEFAULT GETUTCDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    PRIMARY KEY (ProducerID, UserID),
    FOREIGN KEY (ProducerID) REFERENCES Producers(ProducerID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
-- Creazione della tabella Wallet
CREATE TABLE Wallets (
    WalletID INT PRIMARY KEY IDENTITY(1,1), -- ID univoco per il portafoglio
    UserID INT NOT NULL, -- Collegamento all'utente
    Balance DECIMAL(10,2) NOT NULL DEFAULT 0.00, -- Saldo attuale
    Created DATETIME NOT NULL DEFAULT GETDATE(), -- Data di creazione
    Modified DATETIME NULL, -- Data di modifica
    Deleted DATETIME NULL, -- Soft delete
    CONSTRAINT FK_Wallets_Users FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE -- Relazione con la tabella Users
);

-- Creazione della tabella WalletTransactions per la cronologia delle transazioni
CREATE TABLE WalletTransactions (
    TransactionID INT PRIMARY KEY IDENTITY(1,1), -- ID univoco per la transazione
    WalletID INT NOT NULL, -- Collegamento al portafoglio
    Amount DECIMAL(10,2) NOT NULL, -- Importo della transazione (positivo o negativo)
    TransactionType VARCHAR(50) NOT NULL, -- Tipo di transazione (es. "Deposit", "Withdraw", "Refund")
    Description NVARCHAR(255) NULL, -- Descrizione opzionale
    TransactionDate DATETIME NOT NULL DEFAULT GETDATE(), -- Data della transazione
    Created DATETIME NOT NULL DEFAULT GETDATE(), -- Data di creazione
    Modified DATETIME NULL, -- Data di modifica
    Deleted DATETIME NULL, -- Soft delete
    CONSTRAINT FK_WalletTransactions_Wallets FOREIGN KEY (WalletID) REFERENCES Wallets(WalletID) ON DELETE CASCADE -- Relazione con Wallet
);
CREATE TABLE BundleProducts (
    BundleID INT PRIMARY KEY IDENTITY(1,1), -- ID del bundle
    BundleName NVARCHAR(255) NOT NULL, -- Nome del bundle
    Description TEXT NULL, -- Descrizione del bundle
    Price DECIMAL(10,2) NOT NULL, -- Prezzo del bundle
	Moment NVarchar(50) NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL
);
CREATE TABLE BundleItems (
    BundleItemID INT PRIMARY KEY IDENTITY(1,1),
    BundleID INT , -- Bundle associato
    ProductID INT NOT NULL, -- Prodotto nel bundle
    Quantity INT NOT NULL DEFAULT 1, -- Quantità del prodotto nel bundle
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NULL,
    Deleted DATETIME NULL,
    CONSTRAINT FK_BundleItems_Bundles FOREIGN KEY (BundleID) REFERENCES BundleProducts(BundleID) ON DELETE CASCADE,
    CONSTRAINT FK_BundleItems_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);
CREATE TABLE LoggerServer (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
	LogType int NOT NULL,--0 = erorre 1 = warning  2= log 
	LogMessage nvarchar(4000),--messaggio generico
	StackTrace nvarchar(4000),--errore che da il framwork donet
	LogSource nvarchar(255),--classe/metodo che ha generato l'errore
);
CREATE TABLE LoggerClient (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
	LogType int NOT NULL,--0 = erorre 1 = warning  2= log 
	LogMessage nvarchar(4000),--messaggio generico
	StackTrace nvarchar(4000),--errore che da il framwork donet
	LogSource nvarchar(255),--classe/metodo che ha generato l'errore
);

-- Aggiunta di un indice per migliorare le prestazioni delle query sulle transazioni
CREATE INDEX IDX_WalletTransactions_WalletID ON WalletTransactions (WalletID);


INSERT INTO Producers (ProducerName, ContactInfo, Address,PhotoLinkProduttore, Created, Modified, Deleted)
VALUES
('Bar Colonna', 'BarColonna@gmail.com', 'Via mazzini ','forno_image.jpg', GETDATE(), NULL, NULL);


INSERT INTO Schools (SchoolName, Address, ProducerID, Created, Modified, Deleted)
VALUES
('Liceo Fermi Bologna', 'Bologna', 1, GETDATE(), NULL, NULL);

INSERT INTO SchoolClasses (SchoolID, ClassYear, ClassSection, Created, Modified, Deleted)
VALUES
(1, 1, 'N', GETDATE(), NULL, NULL),
(1, 2, 'N', GETDATE(), NULL, NULL),
(1, 3, 'N', GETDATE(), NULL, NULL),
(1, 4, 'N', GETDATE(), NULL, NULL),
(1, 5, 'N', GETDATE(), NULL, NULL);


INSERT INTO [Users] (UserName, Surname, Password, Email,Verified,VerificationToken, Role, RegistrationDate, SchoolClassID, Created, Modified, Deleted)
VALUES
('Admin', 'Admin', '$2a$11$.QsyJMGqIQ3fl1n7Km17teYqzJH/f0rtwq1v2p3cBRihw6MK5/gIS', 'Admin',0,'sdawdsawd', 'Admin', GETDATE(), 1, GETDATE(), NULL, NULL),
('Alice', 'Rossi','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', '37552810',0,'sdawdsawd', 'Student', GETDATE(), 1, GETDATE(), NULL, NULL),
('Luca', 'Bianchi','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', '375352810222',0,'sdawdsawd', 'Student', GETDATE(), 1, GETDATE(), NULL, NULL),
('Marco', 'Neri', '$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', '375528150222',0,'sdawdsawd', 'Teacher', GETDATE(), NULL, GETDATE(), NULL, NULL),
('Alice', 'Rossi','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', '37552821022',0,'sdawdsawd', 'MasterStudent', GETDATE(), 1, GETDATE(), NULL, NULL),
('Luca', 'Bianchi', '$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', '37551281022',0,'sdawdsawd', 'MasterStudent', GETDATE(), 3, GETDATE(), NULL, NULL),
('Marco', 'Verdi', '$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', '37552815022',0,'sdawdsawd', 'MasterStudent', GETDATE(), 2, GETDATE(), NULL, NULL),
('Giulia', 'Gialli', '$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', '37552281022',0,'sdawdsawd', 'MasterStudent', GETDATE(), 2, GETDATE(), NULL, NULL),
('Paolo', 'Neri', '$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', 'paolo',0,'sdawdsawd', 'MasterStudent', GETDATE(), 3, GETDATE(), NULL, NULL),
('Martina', 'Blu', '$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', '3755281022',0,'sdawdsawd', 'MasterStudent', GETDATE(), 3, GETDATE(), NULL, NULL),
('Producer', 'Producer', '$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a', 'Producer',1,'sdawdsawd', 'Producer', GETDATE(), 3, GETDATE(), NULL, NULL);

INSERT INTO Promotions (PromotionName, Description, DiscountPercentage, StartDate, EndDate, Created, Modified, Deleted)
VALUES
('Spring Sale', '10% off on all products', 10.0, '2008-11-11 13:23:44', '2010-11-11 13:23:44', GETDATE(), NULL, NULL),
('Winter Special', '15% off on selected items', 15.0, '2024-11-11 13:23:44', '2025-11-11 13:23:44', GETDATE(), NULL, NULL);


INSERT INTO BundleProducts (BundleName, Description, Price,Moment, Created,Modified,Deleted)
VALUES
('Snack Combo Salato', 'Un set di snack salati con panino e bevanda.', 5.50,'Pranzo', GETDATE(),NUll,NULL),
('Merenda Dolce', 'Un pacchetto con un dolce e una bevanda.', 4.20,'Colazione', GETDATE(),NUll,NULL);



INSERT INTO Products (ProductName,BundleID, Description, Details, Raccomandation, Price, ProducerID, PhotoLinkProdotto, Created, Modified, Deleted)
VALUES

-- Salati
('Panino al Prosciutto',NULL, 'Morbido pane bianco farcito con prosciutto crudo e burro', 'Pane, prosciutto crudo, burro', 'Consumare fresco per mantenere il sapore', 4.50, 1, 'panino_prosciutto.jpg', GETDATE(), NULL, NULL),
('Trancio di Pizza Margherita',NULL, 'Base soffice con pomodoro fresco e mozzarella filante', 'Farina, pomodoro, mozzarella, origano', 'Meglio consumare caldo', 3.00, 1, 'trancio_pizza.jpg', GETDATE(), NULL, NULL),
('Focaccia alle Olive',NULL, 'Focaccia croccante arricchita con olive nere intere', 'Farina, acqua, olive nere, sale', 'Conservare in luogo fresco e asciutto', 2.80, 1, 'focaccia_olive.jpg', GETDATE(), NULL, NULL),
('Arancino al Ragù',NULL, 'Classico arancino siciliano ripieno di ragù e mozzarella', 'Riso, ragù di carne, mozzarella, panatura', 'Consumare caldo', 3.50, 1, 'arancino_ragu.jpg', GETDATE(), NULL, NULL),
('Torta Salata agli Spinaci',NULL, 'Sfoglia croccante con ripieno di spinaci e ricotta', 'Spinaci, ricotta, pasta sfoglia', 'Conservare in frigorifero', 4.00, 1, 'torta_spinaci.jpg', GETDATE(), NULL, NULL),
('Baguette Farcita',NULL, 'Croccante baguette con salame, formaggio e insalata', 'Baguette, salame, formaggio, insalata', 'Consumare fresco', 4.80, 1, 'baguette_farcita.jpg', GETDATE(), NULL, NULL),

-- Dolci
('Cornetto alla Crema',NULL, 'Cornetto sfogliato ripieno di crema pasticcera fresca', 'Farina, burro, crema pasticcera', 'Consumare fresco', 1.80, 1, 'cornetto_crema.jpg', GETDATE(), NULL, NULL),
('Tiramisù Monoporzione',NULL, 'Strati di savoiardi, mascarpone e cacao', 'Savoiardi, mascarpone, caffè, cacao', 'Conservare in frigorifero', 3.50, 1, 'tiramisu.jpg', GETDATE(), NULL, NULL),
('Muffin al Cioccolato',NULL, 'Morbido muffin con gocce di cioccolato fondente', 'Farina, cioccolato fondente, zucchero', 'Conservare in luogo fresco e asciutto', 2.20, 1, 'muffin_cioccolato.jpg', GETDATE(), NULL, NULL),
('Cheesecake ai Frutti di Bosco',NULL, 'Cheesecake con topping di frutti di bosco freschi', 'Formaggio cremoso, biscotti, frutti di bosco', 'Conservare in frigorifero', 4.00, 1, 'cheesecake.jpg', GETDATE(), NULL, NULL),
('Torta della Nonna',NULL, 'Crostata ripiena di crema pasticcera e pinoli', 'Farina, crema pasticcera, pinoli', 'Conservare in luogo fresco', 3.80, 1, 'torta_nonna.jpg', GETDATE(), NULL, NULL),

-- Bevande
('Caffè Espresso',NULL, 'Espresso intenso preparato al momento', 'Caffè', 'Consumare subito dopo la preparazione', 1.20, 1, 'caffe.jpg', GETDATE(), NULL, NULL),
('Cappuccino',NULL, 'Espresso con schiuma di latte morbida e vellutata', 'Caffè, latte', 'Consumare caldo', 1.80, 1, 'cappuccino.jpg', GETDATE(), NULL, NULL),
('Succo di Frutta alla Pesca',NULL, 'Succo naturale con il 100% di polpa di pesca', 'Pesca, acqua, zucchero', 'Agitare bene prima dell’uso', 2.00, 1, 'succo_frutta.jpg', GETDATE(), NULL, NULL),
('Tè Freddo al Limone',NULL, 'Rinfrescante bevanda al tè con un tocco di limone', 'Tè, zucchero, limone', 'Servire freddo', 1.50, 1, 'te_limone.jpg', GETDATE(), NULL, NULL),
('Acqua Naturale 500ml',NULL, 'Bottiglia di acqua naturale fresca', ' ', 'Conservare al riparo dalla luce solare', 1.00, 1, 'acqua.jpg', GETDATE(), NULL, NULL),

-- Fit
('Insalata di Pollo',NULL, 'Pezzi di pollo grigliato con verdure fresche', 'Pollo, verdure miste, olio extravergine di oliva', 'Consumare fresco', 6.00, 1, 'insalata_pollo.jpg', GETDATE(), NULL, NULL),
('Porridge con Frutti Rossi',NULL, 'Avena cotta con latte di mandorla e frutti rossi', 'Avena, latte di mandorla, frutti rossi', 'Conservare in frigorifero', 4.50, 1, 'porridge.jpg', GETDATE(), NULL, NULL),
('Smoothie Verde Detox',NULL, 'Frullato di spinaci, mela verde e cetriolo', 'Spinaci, mela verde, cetriolo', 'Consumare fresco', 3.80, 1, 'smoothie_detox.jpg', GETDATE(), NULL, NULL),

-- Vegani
('Panino Vegano all’Hummus',NULL, 'Pane integrale con hummus, pomodori secchi e rucola', 'Pane integrale, hummus, pomodori secchi, rucola', 'Consumare fresco', 4.00, 1, 'panino_hummus.jpg', GETDATE(), NULL, NULL),
('Cous Cous alle Verdure',NULL, 'Cous cous con zucchine, carote e peperoni', 'Cous cous, zucchine, carote, peperoni', 'Conservare in frigorifero', 5.50, 1, 'couscous.jpg', GETDATE(), NULL, NULL),
('Brownie Vegano al Cioccolato',NULL, 'Dolce senza lattosio e uova, a base di cioccolato fondente', 'Farina di mandorle, cioccolato fondente', 'Conservare in luogo fresco e asciutto', 3.20, 1, 'brownie.jpg', GETDATE(), NULL, NULL),

-- Altri
('Macedonia di Frutta Fresca',NULL, 'Mix di frutta di stagione tagliata a pezzi', 'Frutta fresca', 'Consumare immediatamente', 4.00, 1, 'macedonia.jpg', GETDATE(), NULL, NULL),
('Yogurt con Granola',NULL, 'Yogurt naturale con muesli croccante e miele', 'Yogurt, muesli, miele', 'Conservare in frigorifero', 3.50, 1, 'yogurt_granola.jpg', GETDATE(), NULL, NULL),

('Bundle Colazione',1, 'Bundle Colazione', 'Colazione', 'aa', 10, 1, 'bundle_uno.jpg', GETDATE(), NULL, NULL),
('Bundle Pranzo',2, 'Bundle Pranzo', 'Pranzo', 'aa', 8, 1, 'bundle_due.jpg', GETDATE(), NULL, NULL),
('Bundle Secoda merenda',1, 'Bundle Colazione', 'Colazione', 'aa', 10, 1, 'bundle_uno.jpg', GETDATE(), NULL, NULL),
('Bundle Pranzo Fit',2, 'Bundle Pranzo', 'Pranzo', 'aa', 8, 1, 'bundle_due.jpg', GETDATE(), NULL, NULL);

-- Associazione prodotti ai bundle
INSERT INTO BundleItems (BundleID, ProductID, Quantity, Created)
VALUES
-- Bundle 1: Snack Combo Salato (Panino e Succo)
(1, 1, 1, GETDATE()), -- Panino al Prosciutto
(1, 14, 1, GETDATE()), -- Succo di Frutta alla Pesca

-- Bundle 2: Merenda Dolce (Cornetto + Caffè)
(2, 7, 1, GETDATE()), -- Cornetto alla Crema
(2, 12, 1, GETDATE()); -- Caffè Espresso


-- Inserimenti nella tabella Orders con SchoolClassID
INSERT INTO [Orders] (UserID, SchoolClassID,Status, TotalPrice, Created, Modified, Deleted)
VALUES
--(4, 2, 'Pending', 25.00, GETDATE(), NULL, NULL), -- Ordine associato alla Classe A
--(2, 2,'Active', 15.80, GETDATE(), NULL, NULL), -- Ordine associato alla Classe B
--(3, 2, 'Active', 30.00, GETDATE(), NULL, NULL), -- Ordine associato alla Classe C--
--(4, 3, 'Pending', 30.00, GETDATE(), NULL, NULL), -- Ordine associato alla Classe C
--(9, 3, 'Active', 30.00, GETDATE(), NULL, NULL), -- Ordine associato alla Classe C
(9, 3, 'Active', 30.00, GETDATE(), NULL, NULL);-- Ordine associato alla Classe C
--(9, 3, 'Active', 30.00, GETDATE(), NULL, NULL); -- Ordine associato alla Classe C

INSERT INTO ProductPromotions (ProductID, PromotionID, Created, Modified, Deleted)
VALUES
(1, 1, GETDATE(), GETDATE(), NULL),
(2, 2, GETDATE(), GETDATE(), NULL);
INSERT INTO Categories (CategoryName, Description, Created, Modified, Deleted)
VALUES 

('Salato', 'Prodotti Salati', GETDATE(), NULL, NULL),
('Dolce', 'Dolci vari come torte e biscotti', GETDATE(), NULL, NULL),
('Fit', 'FIt', GETDATE(), NULL, NULL),
('Vegan', 'Prodotti vegani', GETDATE(), NULL, NULL),
('Gluten Free', 'Gluten Free Product', GETDATE(), NULL, NULL),
('Bevande', 'Bevande fresche e calde', GETDATE(), NULL, NULL),
('Altro', 'Altro', GETDATE(), NULL, NULL),
('Primo', 'Prodotti per pranzi e merernde sostanziose', GETDATE(), NULL, NULL),
('Secondo', 'Prodotti per merende semplici', GETDATE(), NULL, NULL),
('Bundle', 'Menu', GETDATE(), NULL, NULL);

INSERT INTO CategoryProducts (ProductID, CategoryID, Created, Modified, Deleted)
VALUES
-- Salato (1)
(1, 1, GETDATE(), NULL, NULL), -- Panino al Prosciutto
(2, 1, GETDATE(), NULL, NULL), -- Trancio di Pizza Margherita
(3, 1, GETDATE(), NULL, NULL), -- Focaccia alle Olive
(4, 1, GETDATE(), NULL, NULL), -- Arancino al Ragù
(5, 1, GETDATE(), NULL, NULL), -- Torta Salata agli Spinaci
(6, 1, GETDATE(), NULL, NULL), -- Baguette Farcita
-- Salato (1)
(1, 8, GETDATE(), NULL, NULL), -- Panino al Prosciutto
(2, 8, GETDATE(), NULL, NULL), -- Trancio di Pizza Margherita
(3, 8, GETDATE(), NULL, NULL), -- Focaccia alle Olive
(4, 8, GETDATE(), NULL, NULL), -- Arancino al Ragù
(5, 8, GETDATE(), NULL, NULL), -- Torta Salata agli Spinaci
(6, 8, GETDATE(), NULL, NULL), -- Baguette Farcita

-- Dolce (2)
(7, 2, GETDATE(), NULL, NULL), -- Cornetto alla Crema
(8, 2, GETDATE(), NULL, NULL), -- Tiramisù Monoporzione
(9, 2, GETDATE(), NULL, NULL), -- Muffin al Cioccolato
(10, 2, GETDATE(), NULL, NULL), -- Cheesecake ai Frutti di Bosco
(11, 2, GETDATE(), NULL, NULL), -- Torta della Nonna
-- Dolce (2)
(7, 9, GETDATE(), NULL, NULL), -- Cornetto alla Crema
(8, 9, GETDATE(), NULL, NULL), -- Tiramisù Monoporzione
(9, 9, GETDATE(), NULL, NULL), -- Muffin al Cioccolato
(10,9, GETDATE(), NULL, NULL), -- Cheesecake ai Frutti di Bosco
(11,9, GETDATE(), NULL, NULL), -- Torta della Nonna


-- Fit (3)
(17, 3, GETDATE(), NULL, NULL), -- Insalata di Pollo
(18, 3, GETDATE(), NULL, NULL), -- Porridge con Frutti Rossi
(19, 3, GETDATE(), NULL, NULL), -- Smoothie Verde Detox

-- Fit (3)
(17, 8, GETDATE(), NULL, NULL), -- Insalata di Pollo
(18, 8, GETDATE(), NULL, NULL), -- Porridge con Frutti Rossi
(19, 8, GETDATE(), NULL, NULL), -- Smoothie Verde Detox
-- Vegan (4)
(20, 4, GETDATE(), NULL, NULL), -- Panino Vegano all’Hummus
(21, 4, GETDATE(), NULL, NULL), -- Cous Cous alle Verdure
(22, 4, GETDATE(), NULL, NULL), -- Brownie Vegano al Cioccolato
-- Vegan (4)
(20, 8, GETDATE(), NULL, NULL), -- Panino Vegano all’Hummus
(21, 8, GETDATE(), NULL, NULL), -- Cous Cous alle Verdure
(22, 8, GETDATE(), NULL, NULL), -- Brownie Vegano al Cioccolato
-- Vegan (5)
(20, 5, GETDATE(), NULL, NULL), -- GLUTEN FREE
(21, 5, GETDATE(), NULL, NULL), -- GLUTEN FREE
(22, 5, GETDATE(), NULL, NULL), -- GLUTEN FREE
-- Vegan (5)
(20, 8, GETDATE(), NULL, NULL), -- GLUTEN FREE
(21, 8, GETDATE(), NULL, NULL), -- GLUTEN FREE
(22, 8, GETDATE(), NULL, NULL), -- GLUTEN FREE
-- Bevande (6)
(12, 6, GETDATE(), NULL, NULL), -- Caffè Espresso
(13, 6, GETDATE(), NULL, NULL), -- Cappuccino
(14, 6, GETDATE(), NULL, NULL), -- Succo di Frutta alla Pesca
(15, 6, GETDATE(), NULL, NULL), -- Tè Freddo al Limone
(16, 6, GETDATE(), NULL, NULL), -- Acqua Naturale 500ml

-- Altro (7)
(23, 7, GETDATE(), NULL, NULL), -- Macedonia di Frutta Fresca
(24, 7, GETDATE(), NULL, NULL), -- Yogurt con Granola
-- Altro (7)
(23, 9, GETDATE(), NULL, NULL), -- Macedonia di Frutta Fresca
(24, 9, GETDATE(), NULL, NULL), -- Yogurt con Granola
-- Bundle (10)
(25, 10, GETDATE(), NULL, NULL), -- Macedonia di Frutta Fresca
(26, 10, GETDATE(), NULL, NULL); -- Yogurt con Granola
-- Inserimenti nella tabella Inventory
INSERT INTO Inventory (ProductID, QuantityAvailable, ReorderLevel, Created)
VALUES
(1, 50, 10, GETDATE()), -- Panino al Prosciutto
(2, 150, 10, GETDATE()), -- Trancio di Pizza Margherita
(3, 120, 10, GETDATE()), -- Focaccia alle Olive
(4, 90, 10, GETDATE()), -- Arancino al Ragù
(5, 80, 10, GETDATE()), -- Torta Salata agli Spinaci
(6, 100, 10, GETDATE()), -- Baguette Farcita
(7, 140, 10, GETDATE()), -- Cornetto alla Crema
(8, 60, 10, GETDATE()), -- Tiramisù Monoporzione
(9, 80, 10, GETDATE()), -- Muffin al Cioccolato
(10, 50, 10, GETDATE()), -- Cheesecake ai Frutti di Bosco
(11, 40, 10, GETDATE()), -- Torta della Nonna
(12, 300, 20, GETDATE()), -- Caffè Espresso
(13, 250, 20, GETDATE()), -- Cappuccino
(14, 200, 20, GETDATE()), -- Succo di Frutta alla Pesca
(15, 180, 20, GETDATE()), -- Tè Freddo al Limone
(16, 500, 20, GETDATE()), -- Acqua Naturale 500ml
(17, 60, 5, GETDATE()), -- Insalata di Pollo
(18, 50, 5, GETDATE()), -- Porridge con Frutti Rossi
(19, 40, 5, GETDATE()), -- Smoothie Verde Detox
(20, 30, 5, GETDATE()), -- Panino Vegano all’Hummus
(21, 25, 5, GETDATE()), -- Cous Cous alle Verdure
(22, 20, 5, GETDATE()), -- Brownie Vegano al Cioccolato
(23, 40, 5, GETDATE()), -- Macedonia di Frutta Fresca
(24, 30, 5, GETDATE()), -- Yogurt con Granola
(25, 40, 5, GETDATE()), -- Bundle
(26, 30, 5, GETDATE()); --Bundle
-- Inserimenti nella tabella InventoryHistory
INSERT INTO InventoryHistory (ProductID, ChangeAmount, ChangeReason, ChangedBy, Created, ChangeDate)
VALUES

(1, 100, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Panino al Prosciutto
(2, 150, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Trancio di Pizza Margherita
(3, 120, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Focaccia alle Olive
(4, 90, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Arancino al Ragù
(5, 80, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Torta Salata agli Spinaci
(6, 100, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Baguette Farcita
(7, 140, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Cornetto alla Crema
(8, 60, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Tiramisù Monoporzione
(9, 80, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Muffin al Cioccolato
(10, 50, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Cheesecake ai Frutti di Bosco
(11, 40, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Torta della Nonna
(12, 300, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Caffè Espresso
(13, 250, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Cappuccino
(14, 200, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Succo di Frutta alla Pesca
(15, 180, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Tè Freddo al Limone
(16, 500, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Acqua Naturale 500ml
(17, 60, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Insalata di Pollo
(18, 50, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Porridge con Frutti Rossi
(19, 40, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Smoothie Verde Detox
(20, 30, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Panino Vegano all’Hummus
(21, 25, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Cous Cous alle Verdure
(22, 20, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Brownie Vegano al Cioccolato
(23, 40, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Macedonia di Frutta Fresca
(24, 30, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Yogurt con Granola
(25, 10, 'Initial Stock', 'System', GETDATE(), GETDATE()), -- Yogurt con Granola
(26, 15, 'Initial Stock', 'System', GETDATE(), GETDATE()); -- Yogurt con Granola




-- Inserimenti nella tabella OrderDetails
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice,DeliveryDate,Recreation,ProductCode, Created)
VALUES
(1, 1, 2, 3.50, '2025-01-18', 'Second','RTD4488', GETDATE()),  -- 2 Cheese Sandwich
(1, 4, 1, 2.20, '2025-01-18', 'First','RTD4465', GETDATE()),  -- 1 Orange Juice
(1, 7, 1, 3.80, '2025-01-18', 'Second','RTD4433', GETDATE());  -- 1 Chocolate Cake

/*
(2, 7, 3, 3.80, '2025-01-27', 'Second','RTD441', GETDATE()), -- 1 Chocolate Cake
(6, 7, 1, 3.80, '2025-01-27', 'Second','RTD422', GETDATE()), -- 1 Chocolate Cake
(7, 7, 1, 3.80, '2025-01-27', 'Second','RTD433', GETDATE()), -- 1 Chocolate Cake
(7, 8, 1, 5.80, '2025-01-27', 'Second','RTD456', GETDATE()), -- 1 Chocolate Cake
(5, 1, 2, 3.50, '2025-01-27', 'First','RTD4423', GETDATE()),  -- 2 Cheese Sandwich
(5, 4, 1, 2.20, '2025-01-27', 'First','RTD4444', GETDATE()), -- 1 Orange Juice


-- Ordine 5 6 (User 4)
(5, 1, 2, 3.50, '2025-01-25', 'First','RTD4423', GETDATE()),  -- 2 Cheese Sandwich
(5, 4, 1, 2.20, '2025-01-25', 'Second','RTD4424', GETDATE()),  -- 1 Orange Juice
(5, 7, 1, 3.80, '2025-01-25', 'Second','RTD4455', GETDATE()), -- 1 Chocolate Cake

(6, 1, 2, 3.50, '2025-01-18', 'First','RTD442', GETDATE()),  -- 2 Cheese Sandwich
(6, 4, 1, 2.20, '2025-01-19', 'First','RTD445', GETDATE()),  -- 1 Orange Juice
(6, 7, 1, 3.80, '2025-01-18', 'Second','RTD4477', GETDATE()),  -- 1 Chocolate Cake
-- Ordine 1 (User 4)
(1, 1, 2, 3.50, '2025-01-18', 'Second','RTD4488', GETDATE()),  -- 2 Cheese Sandwich
(1, 4, 1, 2.20, '2025-01-18', 'First','RTD4465', GETDATE()),  -- 1 Orange Juice
(1, 7, 1, 3.80, '2025-01-18', 'Second','RTD4433', GETDATE()),  -- 1 Chocolate Cake

-- Ordine 2 (User 2)
(2, 2, 3, 2.00, '2025-01-18', 'First','RTD4411', GETDATE()),  -- 3 Salty Pretzels
(2, 8, 2, 2.70, '2025-01-18', 'First','RTD4422', GETDATE()),  -- 2 Croissant
(2, 6, 1, 2.50, '2025-01-18', 'Second','RTD4489', GETDATE()),  -- 1 Iced Tea

-- Ordine 3 (User 3)
(3, 3, 4, 4.00, '2025-01-18', 'Second','RTD4499', GETDATE()), -- 4 Ham Panini
(3, 12, 2, 4.50, '2025-01-18', 'First','RTD4433', GETDATE()), -- 2 Chicken Wrap
(3, 15, 1, 3.20, '2025-01-18', 'Second','RTD4499', GETDATE()), -- 1 Vegan Brownie
-- Ordine 3 (User 3)

-- Ordine 3 (User 3)
(4, 3, 4, 4.00, '2025-01-18', 'Second','RTD4499', GETDATE()), -- 4 Ham Panini
(4, 12, 2, 4.50, '2025-01-19', 'First','RTD4477', GETDATE()), -- 2 Chicken Wrap
(4, 15, 1, 3.20, '2025-01-18', 'Second','RTD4490', GETDATE()); -- 1 Vegan Brownie
--INSERT INTO ShoppingSessions (UserID, Status, TotalAmount, CreatedAt, UpdatedAt)
--VALUES
--(2, 'Active', 15.80, GETDATE(), NULL),  -- Sessione attiva per Alice
--(3, 'Active', 30.00, GETDATE(), NULL),  -- Sessione completata per Luca
--(4, 'Active', 25.00, GETDATE(), GETDATE()), -- Sessione annullata per Marco
--(9, 'Active', 25.00, GETDATE(), GETDATE()); -- Sessione annullata per Marco

--INSERT INTO CartItems (SessionID, ProductID, Quantity, Price, Total, CreatedAt, UpdatedAt)
--VALUES
-- Sessione 1 (Alice)
--(1, 2, 3, 2.00, 6.00, GETDATE(), NULL),  -- 3 Salty Pretzels
--(1, 8, 2, 2.70, 5.40, GETDATE(), NULL),  -- 2 Croissant
--(1, 6, 1, 2.50, 2.50, GETDATE(), NULL),  -- 1 Iced Tea

-- Sessione 2 (Luca)
--(2, 3, 4, 4.00, 16.00, GETDATE(), NULL), -- 4 Ham Panini
--(2, 12, 2, 4.50, 9.00, GETDATE(), NULL), -- 2 Chicken Wrap
--(2, 15, 1, 3.20, 3.20, GETDATE(), NULL), -- 1 Vegan Brownie

-- Sessione 3 (Marco)
--(3, 1, 2, 3.50, 7.00, GETDATE(), NULL),  -- 2 Cheese Sandwich
--(3, 4, 1, 2.20, 2.20, GETDATE(), NULL),  -- 1 Orange Juice
--(3, 7, 1, 3.80, 3.80, GETDATE(), NULL);  -- 1 Chocolate Cake
-- Generazione codici per le classi
*/
INSERT INTO ClassDeliveryCodes (SchoolClassID, Code1, Code2, Created, Modified, Deleted)
VALUES
(1, 'CODE1A', 'CODE2A', GETDATE(), NULL, NULL), -- Classe A
(2, 'CODE1B', 'CODE2B', GETDATE(), NULL, NULL), -- Classe B
(3, 'CODE1C', 'CODE2C', GETDATE(), NULL, NULL), -- Classe C
(4, 'CODE1C', 'CODE2C', GETDATE(), NULL, NULL), -- Classe C
(5, 'CODE1C', 'CODE2C', GETDATE(), NULL, NULL); -- Classe C

-- Log delle conferme da parte dei MasterStudent
--INSERT INTO ClassDeliveryLogs (ClassDeliveryCodeID, UserID, CodeType, Timestamp)
--VALUES
-- Classe A
--(1, 2, 'First', GETDATE()), -- Alice conferma il ritiro del primo codice
--(1, 3, 'Second', GETDATE()), -- Luca conferma il ritiro del secondo codice

-- Classe B
--(2, 4, 'First', GETDATE()), -- Marco conferma il ritiro del primo codice
--(2, 5, 'Second', GETDATE()), -- Giulia conferma il ritiro del secondo codice

-- Classe C
--(3, 6, 'First', GETDATE()), -- Paolo conferma il ritiro del primo codice
--(3, 7, 'Second', GETDATE()); -- Martina conferma il ritiro del secondo codice

Insert INTO Allergens (AllergenName, Description) VALUES
('Glutine', 'Presente in frumento, orzo, segale, ecc.'),
('Lattosio', 'Zucchero presente nel latte e nei suoi derivati.'),
('Uova', 'Allergene presente in prodotti derivati dalle uova.'),
('Frutta a guscio', 'Noci, nocciole, mandorle, ecc.'),
('Crostacei', 'Granchi, gamberi, aragoste, ecc.');
INSERT INTO ProductAllergens (ProductID, AllergenID) VALUES
(1, 1), -- Prodotto 1 contiene glutine
(1, 2), -- Prodotto 1 contiene lattosio
(2, 3); -- Prodotto 2 contiene uova
-- Inserisci una nuova promozione valida per il prodotto 1
INSERT INTO Promotions (PromotionName, Description, DiscountPercentage, StartDate, EndDate, Created, Modified, Deleted)
VALUES
('Autumn Sale', '20% off on selected items', 20.0, CONVERT(datetime, '2024-01-01', 120), CONVERT(datetime, '2024-12-31', 120), GETDATE(), NULL, NULL);

-- Collega la promozione al prodotto 1
INSERT INTO ProductPromotions (ProductID, PromotionID, Created, Modified, Deleted)
VALUES
(25, 2, GETDATE(), NULL, NULL),
(1, 2, GETDATE(), NULL, NULL);
-- Ordine 1 (User 4)
INSERT INTO Wallets (UserID, Balance, Created, Modified, Deleted)
VALUES
(9, 50.00, GETDATE(), NULL, NULL); -- 50.00 è l'importo iniziale assegnato
SELECT P.ProducerName FROM ProducerUsers as PS inner join Producers as P on PS.ProducerID=P.ProducerID WHERE PS.UserID = 11 AND PS.Deleted IS NULL
INSERT INTO ProducerUsers (ProducerID, UserID, Created, Modified, Deleted)
VALUES
(1, 11, GETDATE(), NULL, NULL); -- 50.00 è l'importo iniziale assegnato

#####################################################################################################
select * from Users
UPDATE OrderTrackings
SET RetrievedCode1 = 0
WHERE SchoolClassID =3;

select * from Orders
select * from OrderTrackings
select * from Payments
select * from Producers
select * from ProductPromotions
select * from Products
select * from Promotions
select * from Schools
select * from SchoolClasses
select * from Users
select * from CategoryProducts
select * from Inventory
select * from InventoryHistory
select * from Users
select* from ClassDeliveryCodes
select* from ClassDeliveryLogs
select * from SchoolClasses
select * from Schools
select * from Products
select * from Categories
select * from Inventory
select * from Promotions
select * from Allergens
select * from Orders
select * from OrderDetails
select * from Products
select * from Bundles
select * from BundleProducts
select * from Users
select * from ShoppingSessions
select * from Products
select * from CategoryProducts
select * from Categories
select * from RefreshTokens
select * from CartItems
select*from ShoppingSessions
select * from Wallets


-- Rimozione tabelle esistenti nell'ordine corretto per evitare vincoli FK
DROP TABLE IF EXISTS ClassDeliveryLogs;
DROP TABLE IF EXISTS ClassDeliveryCodes;
DROP TABLE IF EXISTS ProductPromotions;
DROP TABLE IF EXISTS CategoryProducts;
DROP TABLE IF EXISTS InventoryHistory;
DROP TABLE IF EXISTS Inventory;
DROP TABLE IF EXISTS OrderDetails;
DROP TABLE IF EXISTS OrderTrackings;
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS SupportRequests;
DROP TABLE IF EXISTS RefreshTokens;
DROP TABLE IF EXISTS Promotions;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Producers;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS SchoolClasses;
DROP TABLE IF EXISTS Schools;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS CartItems;
DROP TABLE IF EXISTS ShoppingSessions;
DROP TABLE IF EXISTS ProductAllergens;
DROP TABLE IF EXISTS Allergens;
DROP TABLE IF EXISTS OrderProductSchedules;
DROP TABLE IF EXISTS Bundles;
DROP TABLE IF EXISTS BundleProducts;
DROP TABLE IF EXISTS BundleItems;
DROP TABLE IF EXISTS Wallets;
DROP TABLE IF EXISTS WalletTransactions;
DROP TABLE IF EXISTS LoggerServer;
DROP TABLE IF EXISTS LoggerClient;
DROP TABLE IF EXISTS ProducerUsers;



	select * from Categories
	select * from OrderDetails
	select * from Wallets
	select * from WalletTransactions
	select * from ShoppingSessions
select * from Inventory
select * from Cart
select * from Users
select * from Allergens

update ClassDeliveryCodes
set RetrievedCode1=0
where SchoolClassID=3
select * from Token


  WITH ProductPurchaseSummary AS (
    SELECT 
        od.ProductID,
        SUM(od.Quantity) AS TotalQuantity
    FROM 
        Orders o
        INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
    WHERE 
        o.UserID = 9
    GROUP BY 
        od.ProductID
)
SELECT TOP 5 
    p.ProductID,
    p.ProductName,
    p.Description,
    p.PhotoLinkProdotto,
    p.Price,
    i.QuantityAvailable,
    i.ReorderLevel,
    CASE 
        WHEN i.QuantityAvailable <= i.ReorderLevel THEN 1
        ELSE 0
    END AS IsLowStock,
    ps.TotalQuantity
FROM 
    ProductPurchaseSummary ps
    INNER JOIN Products p ON ps.ProductID = p.ProductID
    LEFT JOIN Inventory i ON p.ProductID = i.ProductID
WHERE 
    p.Deleted IS NULL
    AND EXISTS (
        SELECT 1 
        FROM CategoryProducts cp 
        WHERE cp.ProductID = p.ProductID AND cp.Deleted IS NULL
    )
ORDER BY 
    ps.TotalQuantity DESC;

    select * from CategoryProducts
update Users 
set Verified = 1 
where UserName = 'producer'
SELECT DISTINCT Address FROM Schools WHERE Deleted IS NULL
update Users 
set Email = 'Deleted1'
where UserName = 'a'
update Users 
set UserName = 'a'
where Verified = '1'

INSERT INTO Categories(CategoryName, Description, Created) 
                          VALUES('Prova','prova', GETUTCDATE());
                          SELECT CAST(SCOPE_IDENTITY() AS int);
    select * from ProductAllergens
        select * from Allergens
                select * from Allergens

    SELECT CategoryID FROM Categories
                  WHERE CategoryName = 'Salato'
                    AND Deleted IS NULL
	SELECT 
    p.ProductID,
    pr.ProducerName AS ProducerName,
    p.ProductName AS ProductName,
    ISNULL(i.QuantityAvailable, 0) AS QuantityAvailable,
    p.Price AS OriginalPrice,
    prom.DiscountPercentage,
    p.Description,
    p.Details,
    p.Raccomandation,
    p.PhotoLinkProdotto,
    pr.PhotoLinkProduttore,
    (
        SELECT STRING_AGG(c.CategoryName, ', ') 
        FROM CategoryProducts cp
        INNER JOIN Categories c ON cp.CategoryID = c.CategoryID
        WHERE cp.ProductID = p.ProductID AND cp.Deleted IS NULL AND c.Deleted IS NULL
    ) AS Categories,
    (
        SELECT STRING_AGG(a.AllergenName, ', ') 
        FROM ProductAllergens pa
        INNER JOIN Allergens a ON pa.AllergenID = a.AllergenID
        WHERE pa.ProductID = p.ProductID AND a.Deleted IS NULL AND pa.Deleted IS NULL
    ) AS Allergens,
    -- Calcolo del prezzo scontato
    CASE 
        WHEN prom.DiscountPercentage IS NOT NULL THEN 
            ROUND(p.Price * (1 - prom.DiscountPercentage / 100.0), 2)
        ELSE 
            p.Price
    END AS DiscountedPrice
FROM 
    Products p
INNER JOIN 
    Producers pr ON p.ProducerID = pr.ProducerID
LEFT JOIN 
    Inventory i ON p.ProductID = i.ProductID
LEFT JOIN 
    (
        SELECT 
            pp.ProductID,
            MAX(prom.DiscountPercentage) AS DiscountPercentage -- Prendi la promozione con lo sconto maggiore
        FROM 
            ProductPromotions pp
        INNER JOIN 
            Promotions prom ON pp.PromotionID = prom.PromotionID 
        WHERE 
            prom.Deleted IS NULL
            AND prom.StartDate <= GETDATE() 
            AND (prom.EndDate IS NULL OR prom.EndDate >= GETDATE())
        GROUP BY 
            pp.ProductID
    ) prom ON p.ProductID = prom.ProductID
WHERE 
    p.ProductID = 1 -- Sostituisci con il ProductID desiderato
    AND p.Deleted IS NULL;