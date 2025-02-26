CREATE DATABASE  IF NOT EXISTS `db_snackupproject` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `db_snackupproject`;
-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: db_snackupproject
-- ------------------------------------------------------
-- Server version	8.0.37

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `allergens`
--

DROP TABLE IF EXISTS `allergens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `allergens` (
  `AllergenID` int NOT NULL AUTO_INCREMENT,
  `AllergenName` varchar(255) NOT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`AllergenID`),
  UNIQUE KEY `AllergenName` (`AllergenName`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `allergens`
--

LOCK TABLES `allergens` WRITE;
/*!40000 ALTER TABLE `allergens` DISABLE KEYS */;
INSERT INTO `allergens` VALUES (1,'Glutine','Presente in frumento, orzo, segale, ecc.','2025-02-25 22:29:52',NULL,NULL),(2,'Lattosio','Zucchero presente nel latte e nei suoi derivati.','2025-02-25 22:29:52',NULL,NULL),(3,'Uova','Allergene presente in prodotti derivati dalle uova.','2025-02-25 22:29:52',NULL,NULL),(4,'Frutta a guscio','Noci, nocciole, mandorle, ecc.','2025-02-25 22:29:52',NULL,NULL),(5,'Crostacei','Granchi, gamberi, aragoste, ecc.','2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `allergens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bundleitems`
--

DROP TABLE IF EXISTS `bundleitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bundleitems` (
  `BundleItemID` int NOT NULL AUTO_INCREMENT,
  `BundleID` int DEFAULT NULL,
  `ProductID` int NOT NULL,
  `Quantity` int NOT NULL DEFAULT '1',
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`BundleItemID`),
  KEY `FK_BundleItems_Bundles` (`BundleID`),
  KEY `FK_BundleItems_Products` (`ProductID`),
  CONSTRAINT `FK_BundleItems_Bundles` FOREIGN KEY (`BundleID`) REFERENCES `bundleproducts` (`BundleID`) ON DELETE CASCADE,
  CONSTRAINT `FK_BundleItems_Products` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bundleitems`
--

LOCK TABLES `bundleitems` WRITE;
/*!40000 ALTER TABLE `bundleitems` DISABLE KEYS */;
INSERT INTO `bundleitems` VALUES (1,1,1,1,'2025-02-25 22:29:52',NULL,NULL),(2,1,14,1,'2025-02-25 22:29:52',NULL,NULL),(3,2,7,1,'2025-02-25 22:29:52',NULL,NULL),(4,2,12,1,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `bundleitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bundleproducts`
--

DROP TABLE IF EXISTS `bundleproducts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bundleproducts` (
  `BundleID` int NOT NULL AUTO_INCREMENT,
  `BundleName` varchar(255) NOT NULL,
  `Description` text,
  `Price` decimal(10,2) NOT NULL,
  `Moment` varchar(50) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`BundleID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bundleproducts`
--

LOCK TABLES `bundleproducts` WRITE;
/*!40000 ALTER TABLE `bundleproducts` DISABLE KEYS */;
INSERT INTO `bundleproducts` VALUES (1,'Snack Combo Salato','Un set di snack salati con panino e bevanda.',5.50,'Pranzo','2025-02-25 22:29:52',NULL,NULL),(2,'Merenda Dolce','Un pacchetto con un dolce e una bevanda.',4.20,'Colazione','2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `bundleproducts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cartitems`
--

DROP TABLE IF EXISTS `cartitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cartitems` (
  `CartItemID` int NOT NULL AUTO_INCREMENT,
  `SessionID` int NOT NULL,
  `ProductID` int NOT NULL,
  `Quantity` int NOT NULL DEFAULT '1',
  `Price` decimal(10,2) NOT NULL,
  `Total` decimal(10,2) NOT NULL,
  `DeliveryDate` datetime DEFAULT NULL,
  `Recreation` varchar(50) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `DeletedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`CartItemID`),
  KEY `SessionID` (`SessionID`),
  KEY `ProductID` (`ProductID`),
  CONSTRAINT `cartitems_ibfk_1` FOREIGN KEY (`SessionID`) REFERENCES `shoppingsessions` (`SessionID`) ON DELETE CASCADE,
  CONSTRAINT `cartitems_ibfk_2` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cartitems`
--

LOCK TABLES `cartitems` WRITE;
/*!40000 ALTER TABLE `cartitems` DISABLE KEYS */;
/*!40000 ALTER TABLE `cartitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `CategoryID` int NOT NULL AUTO_INCREMENT,
  `CategoryName` varchar(255) NOT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`CategoryID`),
  UNIQUE KEY `CategoryName` (`CategoryName`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Salato','Prodotti Salati','2025-02-25 22:29:52',NULL,NULL),(2,'Dolce','Dolci vari come torte e biscotti','2025-02-25 22:29:52',NULL,NULL),(3,'Fit','Fit','2025-02-25 22:29:52',NULL,NULL),(4,'Vegan','Prodotti vegani','2025-02-25 22:29:52',NULL,NULL),(5,'Gluten Free','Gluten Free Product','2025-02-25 22:29:52',NULL,NULL),(6,'Bevande','Bevande fresche e calde','2025-02-25 22:29:52',NULL,NULL),(7,'Altro','Altro','2025-02-25 22:29:52',NULL,NULL),(8,'Primo','Prodotti per pranzi e merernde sostanziose','2025-02-25 22:29:52',NULL,NULL),(9,'Secondo','Prodotti per merende semplici','2025-02-25 22:29:52',NULL,NULL),(10,'Bundle','Menu','2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categoryproducts`
--

DROP TABLE IF EXISTS `categoryproducts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categoryproducts` (
  `ProductID` int NOT NULL,
  `CategoryID` int NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  KEY `ProductID` (`ProductID`),
  KEY `CategoryID` (`CategoryID`),
  CONSTRAINT `categoryproducts_ibfk_1` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`),
  CONSTRAINT `categoryproducts_ibfk_2` FOREIGN KEY (`CategoryID`) REFERENCES `categories` (`CategoryID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categoryproducts`
--

LOCK TABLES `categoryproducts` WRITE;
/*!40000 ALTER TABLE `categoryproducts` DISABLE KEYS */;
INSERT INTO `categoryproducts` VALUES (1,1,'2025-02-25 22:29:52',NULL,NULL),(2,1,'2025-02-25 22:29:52',NULL,NULL),(3,1,'2025-02-25 22:29:52',NULL,NULL),(4,1,'2025-02-25 22:29:52',NULL,NULL),(5,1,'2025-02-25 22:29:52',NULL,NULL),(6,1,'2025-02-25 22:29:52',NULL,NULL),(1,8,'2025-02-25 22:29:52',NULL,NULL),(2,8,'2025-02-25 22:29:52',NULL,NULL),(3,8,'2025-02-25 22:29:52',NULL,NULL),(4,8,'2025-02-25 22:29:52',NULL,NULL),(5,8,'2025-02-25 22:29:52',NULL,NULL),(6,8,'2025-02-25 22:29:52',NULL,NULL),(7,2,'2025-02-25 22:29:52',NULL,NULL),(8,2,'2025-02-25 22:29:52',NULL,NULL),(9,2,'2025-02-25 22:29:52',NULL,NULL),(10,2,'2025-02-25 22:29:52',NULL,NULL),(11,2,'2025-02-25 22:29:52',NULL,NULL),(7,9,'2025-02-25 22:29:52',NULL,NULL),(8,9,'2025-02-25 22:29:52',NULL,NULL),(9,9,'2025-02-25 22:29:52',NULL,NULL),(10,9,'2025-02-25 22:29:52',NULL,NULL),(11,9,'2025-02-25 22:29:52',NULL,NULL),(17,3,'2025-02-25 22:29:52',NULL,NULL),(18,3,'2025-02-25 22:29:52',NULL,NULL),(19,3,'2025-02-25 22:29:52',NULL,NULL),(17,8,'2025-02-25 22:29:52',NULL,NULL),(18,8,'2025-02-25 22:29:52',NULL,NULL),(19,8,'2025-02-25 22:29:52',NULL,NULL),(20,4,'2025-02-25 22:29:52',NULL,NULL),(21,4,'2025-02-25 22:29:52',NULL,NULL),(22,4,'2025-02-25 22:29:52',NULL,NULL),(20,8,'2025-02-25 22:29:52',NULL,NULL),(21,8,'2025-02-25 22:29:52',NULL,NULL),(22,8,'2025-02-25 22:29:52',NULL,NULL),(20,5,'2025-02-25 22:29:52',NULL,NULL),(21,5,'2025-02-25 22:29:52',NULL,NULL),(22,5,'2025-02-25 22:29:52',NULL,NULL),(20,8,'2025-02-25 22:29:52',NULL,NULL),(21,8,'2025-02-25 22:29:52',NULL,NULL),(22,8,'2025-02-25 22:29:52',NULL,NULL),(12,6,'2025-02-25 22:29:52',NULL,NULL),(13,6,'2025-02-25 22:29:52',NULL,NULL),(14,6,'2025-02-25 22:29:52',NULL,NULL),(15,6,'2025-02-25 22:29:52',NULL,NULL),(16,6,'2025-02-25 22:29:52',NULL,NULL),(23,7,'2025-02-25 22:29:52',NULL,NULL),(24,7,'2025-02-25 22:29:52',NULL,NULL),(23,9,'2025-02-25 22:29:52',NULL,NULL),(24,9,'2025-02-25 22:29:52',NULL,NULL),(25,10,'2025-02-25 22:29:52',NULL,NULL),(26,10,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `categoryproducts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `classdeliverycodes`
--

DROP TABLE IF EXISTS `classdeliverycodes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `classdeliverycodes` (
  `ClassDeliveryCodeID` int NOT NULL AUTO_INCREMENT,
  `SchoolClassID` int NOT NULL,
  `Code1` varchar(10) NOT NULL,
  `Code2` varchar(10) NOT NULL,
  `RetrievedCode1` tinyint(1) DEFAULT '0',
  `RetrievedCode2` tinyint(1) DEFAULT '0',
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`ClassDeliveryCodeID`),
  KEY `FK_ClassDeliveryCodes_SchoolClasses` (`SchoolClassID`),
  CONSTRAINT `FK_ClassDeliveryCodes_SchoolClasses` FOREIGN KEY (`SchoolClassID`) REFERENCES `schoolclasses` (`SchoolClassID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classdeliverycodes`
--

LOCK TABLES `classdeliverycodes` WRITE;
/*!40000 ALTER TABLE `classdeliverycodes` DISABLE KEYS */;
INSERT INTO `classdeliverycodes` VALUES (1,1,'CODE1A','CODE2A',0,0,'2025-02-25 22:29:52',NULL,NULL),(2,2,'CODE1B','CODE2B',0,0,'2025-02-25 22:29:52',NULL,NULL),(3,3,'CODE1C','CODE2C',0,0,'2025-02-25 22:29:52',NULL,NULL),(4,4,'CODE1C','CODE2C',0,0,'2025-02-25 22:29:52',NULL,NULL),(5,5,'CODE1C','CODE2C',0,0,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `classdeliverycodes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `classdeliverylogs`
--

DROP TABLE IF EXISTS `classdeliverylogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `classdeliverylogs` (
  `LogID` int NOT NULL AUTO_INCREMENT,
  `ClassDeliveryCodeID` int NOT NULL,
  `UserID` int NOT NULL,
  `CodeType` varchar(10) NOT NULL,
  `Timestamp` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`LogID`),
  KEY `ClassDeliveryCodeID` (`ClassDeliveryCodeID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `classdeliverylogs_ibfk_1` FOREIGN KEY (`ClassDeliveryCodeID`) REFERENCES `classdeliverycodes` (`ClassDeliveryCodeID`) ON DELETE CASCADE,
  CONSTRAINT `classdeliverylogs_ibfk_2` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classdeliverylogs`
--

LOCK TABLES `classdeliverylogs` WRITE;
/*!40000 ALTER TABLE `classdeliverylogs` DISABLE KEYS */;
/*!40000 ALTER TABLE `classdeliverylogs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventory` (
  `ProductID` int NOT NULL,
  `QuantityAvailable` int NOT NULL,
  `QuantityReserved` int DEFAULT '0',
  `ReorderLevel` int DEFAULT '10',
  `LastUpdated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`ProductID`),
  CONSTRAINT `FK_Inventory_Products` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory`
--

LOCK TABLES `inventory` WRITE;
/*!40000 ALTER TABLE `inventory` DISABLE KEYS */;
INSERT INTO `inventory` VALUES (1,50,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(2,150,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(3,120,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(4,90,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(5,80,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(6,100,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(7,140,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(8,60,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(9,80,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(10,50,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(11,40,0,10,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(12,300,0,20,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(13,250,0,20,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(14,200,0,20,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(15,180,0,20,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(16,500,0,20,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(17,60,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(18,50,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(19,40,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(20,30,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(21,25,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(22,20,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(23,40,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(24,30,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(25,40,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL),(26,30,0,5,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventoryhistory`
--

DROP TABLE IF EXISTS `inventoryhistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventoryhistory` (
  `HistoryID` int NOT NULL AUTO_INCREMENT,
  `ProductID` int NOT NULL,
  `ChangeAmount` int NOT NULL,
  `ChangeReason` varchar(255) NOT NULL,
  `ChangedBy` varchar(100) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  `ChangeDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`HistoryID`),
  KEY `ProductID` (`ProductID`),
  CONSTRAINT `inventoryhistory_ibfk_1` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventoryhistory`
--

LOCK TABLES `inventoryhistory` WRITE;
/*!40000 ALTER TABLE `inventoryhistory` DISABLE KEYS */;
INSERT INTO `inventoryhistory` VALUES (1,1,100,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(2,2,150,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(3,3,120,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(4,4,90,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(5,5,80,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(6,6,100,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(7,7,140,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(8,8,60,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(9,9,80,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(10,10,50,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(11,11,40,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(12,12,300,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(13,13,250,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(14,14,200,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(15,15,180,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(16,16,500,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(17,17,60,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(18,18,50,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(19,19,40,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(20,20,30,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(21,21,25,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(22,22,20,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(23,23,40,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(24,24,30,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(25,25,10,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52'),(26,26,15,'Initial Stock','System','2025-02-25 22:29:52',NULL,NULL,'2025-02-25 22:29:52');
/*!40000 ALTER TABLE `inventoryhistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loggerclient`
--

DROP TABLE IF EXISTS `loggerclient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `loggerclient` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LogType` int NOT NULL,
  `LogMessage` varchar(4000) DEFAULT NULL,
  `StackTrace` varchar(4000) DEFAULT NULL,
  `LogSource` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loggerclient`
--

LOCK TABLES `loggerclient` WRITE;
/*!40000 ALTER TABLE `loggerclient` DISABLE KEYS */;
/*!40000 ALTER TABLE `loggerclient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loggerserver`
--

DROP TABLE IF EXISTS `loggerserver`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `loggerserver` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LogType` int NOT NULL,
  `LogMessage` varchar(4000) DEFAULT NULL,
  `StackTrace` varchar(4000) DEFAULT NULL,
  `LogSource` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loggerserver`
--

LOCK TABLES `loggerserver` WRITE;
/*!40000 ALTER TABLE `loggerserver` DISABLE KEYS */;
/*!40000 ALTER TABLE `loggerserver` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orderdetails`
--

DROP TABLE IF EXISTS `orderdetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orderdetails` (
  `DetailID` int NOT NULL AUTO_INCREMENT,
  `OrderID` int NOT NULL,
  `ProductID` int NOT NULL,
  `Quantity` int NOT NULL,
  `UnitPrice` decimal(10,2) NOT NULL,
  `DeliveryDate` date NOT NULL,
  `Recreation` varchar(10) NOT NULL,
  `ProductCode` varchar(10) NOT NULL,
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`DetailID`),
  KEY `FK_OrderDetails_Orders` (`OrderID`),
  KEY `FK_OrderDetails_Products` (`ProductID`),
  CONSTRAINT `FK_OrderDetails_Orders` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`),
  CONSTRAINT `FK_OrderDetails_Products` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orderdetails`
--

LOCK TABLES `orderdetails` WRITE;
/*!40000 ALTER TABLE `orderdetails` DISABLE KEYS */;
INSERT INTO `orderdetails` VALUES (1,1,1,2,3.50,'2025-01-18','Second','RTD4488','2025-02-25 22:29:52',NULL,NULL),(2,1,4,1,2.20,'2025-01-18','First','RTD4465','2025-02-25 22:29:52',NULL,NULL),(3,1,7,1,3.80,'2025-01-18','Second','RTD4433','2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `orderdetails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `OrderID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `SchoolClassID` int NOT NULL,
  `Status` varchar(50) NOT NULL,
  `TotalPrice` decimal(10,2) DEFAULT NULL,
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`OrderID`),
  KEY `FK_Orders_Users` (`UserID`),
  KEY `FK_Orders_SchoolClasses` (`SchoolClassID`),
  CONSTRAINT `FK_Orders_SchoolClasses` FOREIGN KEY (`SchoolClassID`) REFERENCES `schoolclasses` (`SchoolClassID`) ON DELETE CASCADE,
  CONSTRAINT `FK_Orders_Users` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,9,3,'Active',30.00,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ordertrackings`
--

DROP TABLE IF EXISTS `ordertrackings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ordertrackings` (
  `OrderTrackingID` int NOT NULL AUTO_INCREMENT,
  `OrderID` int NOT NULL,
  `Status` varchar(50) NOT NULL,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`OrderTrackingID`),
  KEY `FK_OrderTrackings_Orders` (`OrderID`),
  CONSTRAINT `FK_OrderTrackings_Orders` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ordertrackings`
--

LOCK TABLES `ordertrackings` WRITE;
/*!40000 ALTER TABLE `ordertrackings` DISABLE KEYS */;
/*!40000 ALTER TABLE `ordertrackings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payments`
--

DROP TABLE IF EXISTS `payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payments` (
  `PaymentID` int NOT NULL AUTO_INCREMENT,
  `OrderID` int DEFAULT NULL,
  `SubscriptionID` int DEFAULT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `PaymentMethod` varchar(50) NOT NULL,
  `PaymentDate` datetime NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`PaymentID`),
  KEY `FK_Payments_Orders` (`OrderID`),
  CONSTRAINT `FK_Payments_Orders` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payments`
--

LOCK TABLES `payments` WRITE;
/*!40000 ALTER TABLE `payments` DISABLE KEYS */;
/*!40000 ALTER TABLE `payments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `producers`
--

DROP TABLE IF EXISTS `producers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `producers` (
  `ProducerID` int NOT NULL AUTO_INCREMENT,
  `ProducerName` varchar(255) NOT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `ContactInfo` varchar(255) DEFAULT NULL,
  `PhotoLinkProduttore` varchar(500) DEFAULT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`ProducerID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `producers`
--

LOCK TABLES `producers` WRITE;
/*!40000 ALTER TABLE `producers` DISABLE KEYS */;
INSERT INTO `producers` VALUES (1,'Bar Colonna','Via mazzini','BarColonna@gmail.com','forno_image.jpg','2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `producers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `producerusers`
--

DROP TABLE IF EXISTS `producerusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `producerusers` (
  `ProducerID` int NOT NULL,
  `UserID` int NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`ProducerID`,`UserID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `producerusers_ibfk_1` FOREIGN KEY (`ProducerID`) REFERENCES `producers` (`ProducerID`) ON DELETE CASCADE,
  CONSTRAINT `producerusers_ibfk_2` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `producerusers`
--

LOCK TABLES `producerusers` WRITE;
/*!40000 ALTER TABLE `producerusers` DISABLE KEYS */;
INSERT INTO `producerusers` VALUES (1,11,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `producerusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productallergens`
--

DROP TABLE IF EXISTS `productallergens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productallergens` (
  `ProductAllergenID` int NOT NULL AUTO_INCREMENT,
  `ProductID` int NOT NULL,
  `AllergenID` int NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`ProductAllergenID`),
  UNIQUE KEY `uniq_Product_Allergen` (`ProductID`,`AllergenID`),
  KEY `AllergenID` (`AllergenID`),
  CONSTRAINT `productallergens_ibfk_1` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`) ON DELETE CASCADE,
  CONSTRAINT `productallergens_ibfk_2` FOREIGN KEY (`AllergenID`) REFERENCES `allergens` (`AllergenID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productallergens`
--

LOCK TABLES `productallergens` WRITE;
/*!40000 ALTER TABLE `productallergens` DISABLE KEYS */;
INSERT INTO `productallergens` VALUES (1,1,1,'2025-02-25 22:29:52',NULL,NULL),(2,1,2,'2025-02-25 22:29:52',NULL,NULL),(3,2,3,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `productallergens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productpromotions`
--

DROP TABLE IF EXISTS `productpromotions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productpromotions` (
  `ProductPromotionID` int NOT NULL AUTO_INCREMENT,
  `ProductID` int NOT NULL,
  `PromotionID` int NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`ProductPromotionID`),
  KEY `FK_ProductPromotions_Products` (`ProductID`),
  KEY `FK_ProductPromotions_Promotions` (`PromotionID`),
  CONSTRAINT `FK_ProductPromotions_Products` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`) ON DELETE CASCADE,
  CONSTRAINT `FK_ProductPromotions_Promotions` FOREIGN KEY (`PromotionID`) REFERENCES `promotions` (`PromotionID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productpromotions`
--

LOCK TABLES `productpromotions` WRITE;
/*!40000 ALTER TABLE `productpromotions` DISABLE KEYS */;
INSERT INTO `productpromotions` VALUES (1,1,1,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL),(2,2,2,'2025-02-25 22:29:52','2025-02-25 22:29:52',NULL),(3,25,2,'2025-02-25 22:29:52',NULL,NULL),(4,1,2,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `productpromotions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `ProductID` int NOT NULL AUTO_INCREMENT,
  `BundleID` int DEFAULT NULL,
  `ProductName` varchar(255) NOT NULL,
  `Description` text,
  `Details` text,
  `Raccomandation` text,
  `Price` decimal(10,2) DEFAULT NULL,
  `PhotoLinkProdotto` varchar(500) DEFAULT NULL,
  `ProducerID` int NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`ProductID`),
  KEY `FK_Products_Producers` (`ProducerID`),
  CONSTRAINT `FK_Products_Producers` FOREIGN KEY (`ProducerID`) REFERENCES `producers` (`ProducerID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,NULL,'Panino al Prosciutto','Morbido pane bianco farcito con prosciutto crudo e burro','Pane, prosciutto crudo, burro','Consumare fresco per mantenere il sapore',4.50,'panino_prosciutto.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(2,NULL,'Trancio di Pizza Margherita','Base soffice con pomodoro fresco e mozzarella filante','Farina, pomodoro, mozzarella, origano','Meglio consumare caldo',3.00,'trancio_pizza.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(3,NULL,'Focaccia alle Olive','Focaccia croccante arricchita con olive nere intere','Farina, acqua, olive nere, sale','Conservare in luogo fresco e asciutto',2.80,'focaccia_olive.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(4,NULL,'Arancino al Ragù','Classico arancino siciliano ripieno di ragù e mozzarella','Riso, ragù di carne, mozzarella, panatura','Consumare caldo',3.50,'arancino_ragu.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(5,NULL,'Torta Salata agli Spinaci','Sfoglia croccante con ripieno di spinaci e ricotta','Spinaci, ricotta, pasta sfoglia','Conservare in frigorifero',4.00,'torta_spinaci.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(6,NULL,'Baguette Farcita','Croccante baguette con salame, formaggio e insalata','Baguette, salame, formaggio, insalata','Consumare fresco',4.80,'baguette_farcita.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(7,NULL,'Cornetto alla Crema','Cornetto sfogliato ripieno di crema pasticcera fresca','Farina, burro, crema pasticcera','Consumare fresco',1.80,'cornetto_crema.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(8,NULL,'Tiramisù Monoporzione','Strati di savoiardi, mascarpone e cacao','Savoiardi, mascarpone, caffè, cacao','Conservare in frigorifero',3.50,'tiramisu.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(9,NULL,'Muffin al Cioccolato','Morbido muffin con gocce di cioccolato fondente','Farina, cioccolato fondente, zucchero','Conservare in luogo fresco e asciutto',2.20,'muffin_cioccolato.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(10,NULL,'Cheesecake ai Frutti di Bosco','Cheesecake con topping di frutti di bosco freschi','Formaggio cremoso, biscotti, frutti di bosco','Conservare in frigorifero',4.00,'cheesecake.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(11,NULL,'Torta della Nonna','Crostata ripiena di crema pasticcera e pinoli','Farina, crema pasticcera, pinoli','Conservare in luogo fresco',3.80,'torta_nonna.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(12,NULL,'Caffè Espresso','Espresso intenso preparato al momento','Caffè','Consumare subito dopo la preparazione',1.20,'caffe.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(13,NULL,'Cappuccino','Espresso con schiuma di latte morbida e vellutata','Caffè, latte','Consumare caldo',1.80,'cappuccino.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(14,NULL,'Succo di Frutta alla Pesca','Succo naturale con il 100% di polpa di pesca','Pesca, acqua, zucchero','Agitare bene prima dell’uso',2.00,'succo_frutta.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(15,NULL,'Tè Freddo al Limone','Rinfrescante bevanda al tè con un tocco di limone','Tè, zucchero, limone','Servire freddo',1.50,'te_limone.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(16,NULL,'Acqua Naturale 500ml','Bottiglia di acqua naturale fresca',' ','Conservare al riparo dalla luce solare',1.00,'acqua.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(17,NULL,'Insalata di Pollo','Pezzi di pollo grigliato con verdure fresche','Pollo, verdure miste, olio extravergine di oliva','Consumare fresco',6.00,'insalata_pollo.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(18,NULL,'Porridge con Frutti Rossi','Avena cotta con latte di mandorla e frutti rossi','Avena, latte di mandorla, frutti rossi','Conservare in frigorifero',4.50,'porridge.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(19,NULL,'Smoothie Verde Detox','Frullato di spinaci, mela verde e cetriolo','Spinaci, mela verde, cetriolo','Consumare fresco',3.80,'smoothie_detox.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(20,NULL,'Panino Vegano all’Hummus','Pane integrale con hummus, pomodori secchi e rucola','Pane integrale, hummus, pomodori secchi, rucola','Consumare fresco',4.00,'panino_hummus.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(21,NULL,'Cous Cous alle Verdure','Cous cous con zucchine, carote e peperoni','Cous cous, zucchine, carote, peperoni','Conservare in frigorifero',5.50,'couscous.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(22,NULL,'Brownie Vegano al Cioccolato','Dolce senza lattosio e uova, a base di cioccolato fondente','Farina di mandorle, cioccolato fondente','Conservare in luogo fresco e asciutto',3.20,'brownie.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(23,NULL,'Macedonia di Frutta Fresca','Mix di frutta di stagione tagliata a pezzi','Frutta fresca','Consumare immediatamente',4.00,'macedonia.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(24,NULL,'Yogurt con Granola','Yogurt naturale con muesli croccante e miele','Yogurt, muesli, miele','Conservare in frigorifero',3.50,'yogurt_granola.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(25,1,'Bundle Colazione','Bundle Colazione','Colazione','aa',10.00,'bundle_uno.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(26,2,'Bundle Pranzo','Bundle Pranzo','Pranzo','aa',8.00,'bundle_due.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(27,1,'Bundle Secoda merenda','Bundle Colazione','Colazione','aa',10.00,'bundle_uno.jpg',1,'2025-02-25 22:29:52',NULL,NULL),(28,2,'Bundle Pranzo Fit','Bundle Pranzo','Pranzo','aa',8.00,'bundle_due.jpg',1,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `promotions`
--

DROP TABLE IF EXISTS `promotions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `promotions` (
  `PromotionID` int NOT NULL AUTO_INCREMENT,
  `PromotionName` varchar(255) NOT NULL,
  `Description` text,
  `DiscountPercentage` decimal(5,2) NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`PromotionID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `promotions`
--

LOCK TABLES `promotions` WRITE;
/*!40000 ALTER TABLE `promotions` DISABLE KEYS */;
INSERT INTO `promotions` VALUES (1,'Spring Sale','10% off on all products',10.00,'2008-11-11 13:23:44','2010-11-11 13:23:44','2025-02-25 22:29:52',NULL,NULL),(2,'Winter Special','15% off on selected items',15.00,'2024-11-11 13:23:44','2025-11-11 13:23:44','2025-02-25 22:29:52',NULL,NULL),(3,'Autumn Sale','20% off on selected items',20.00,'2024-01-01 00:00:00','2024-12-31 00:00:00','2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `promotions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `refreshtokens`
--

DROP TABLE IF EXISTS `refreshtokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `refreshtokens` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `Token` varchar(255) NOT NULL,
  `ExpirationDate` datetime NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsRevoked` tinyint(1) NOT NULL DEFAULT '0',
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_RefreshTokens_Users` (`UserID`),
  CONSTRAINT `FK_RefreshTokens_Users` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `refreshtokens`
--

LOCK TABLES `refreshtokens` WRITE;
/*!40000 ALTER TABLE `refreshtokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `refreshtokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schoolclasses`
--

DROP TABLE IF EXISTS `schoolclasses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `schoolclasses` (
  `SchoolClassID` int NOT NULL AUTO_INCREMENT,
  `SchoolID` int NOT NULL,
  `ClassYear` int NOT NULL,
  `ClassSection` varchar(10) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`SchoolClassID`),
  KEY `FK_SchoolClasses_Schools` (`SchoolID`),
  CONSTRAINT `FK_SchoolClasses_Schools` FOREIGN KEY (`SchoolID`) REFERENCES `schools` (`SchoolID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schoolclasses`
--

LOCK TABLES `schoolclasses` WRITE;
/*!40000 ALTER TABLE `schoolclasses` DISABLE KEYS */;
INSERT INTO `schoolclasses` VALUES (1,1,1,'N','2025-02-25 22:29:52',NULL,NULL),(2,1,2,'N','2025-02-25 22:29:52',NULL,NULL),(3,1,3,'N','2025-02-25 22:29:52',NULL,NULL),(4,1,4,'N','2025-02-25 22:29:52',NULL,NULL),(5,1,5,'N','2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `schoolclasses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schools`
--

DROP TABLE IF EXISTS `schools`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `schools` (
  `SchoolID` int NOT NULL AUTO_INCREMENT,
  `SchoolName` varchar(255) NOT NULL,
  `Address` varchar(255) NOT NULL,
  `ProducerID` int DEFAULT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`SchoolID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schools`
--

LOCK TABLES `schools` WRITE;
/*!40000 ALTER TABLE `schools` DISABLE KEYS */;
INSERT INTO `schools` VALUES (1,'Liceo Fermi Bologna','Bologna',1,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `schools` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shoppingsessions`
--

DROP TABLE IF EXISTS `shoppingsessions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shoppingsessions` (
  `SessionID` int NOT NULL AUTO_INCREMENT,
  `UserID` int DEFAULT NULL,
  `Status` varchar(20) NOT NULL DEFAULT 'Active',
  `TotalAmount` decimal(10,2) NOT NULL DEFAULT '0.00',
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`SessionID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `shoppingsessions_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shoppingsessions`
--

LOCK TABLES `shoppingsessions` WRITE;
/*!40000 ALTER TABLE `shoppingsessions` DISABLE KEYS */;
/*!40000 ALTER TABLE `shoppingsessions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `supportrequests`
--

DROP TABLE IF EXISTS `supportrequests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `supportrequests` (
  `SupportRequestID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `OrderID` int DEFAULT NULL,
  `Subject` varchar(255) NOT NULL,
  `Description` text NOT NULL,
  `Status` varchar(50) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`SupportRequestID`),
  KEY `FK_SupportRequests_Users` (`UserID`),
  KEY `FK_SupportRequests_Orders` (`OrderID`),
  CONSTRAINT `FK_SupportRequests_Orders` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`),
  CONSTRAINT `FK_SupportRequests_Users` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `supportrequests`
--

LOCK TABLES `supportrequests` WRITE;
/*!40000 ALTER TABLE `supportrequests` DISABLE KEYS */;
/*!40000 ALTER TABLE `supportrequests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `UserName` varchar(100) NOT NULL,
  `Surname` varchar(100) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Verified` int NOT NULL,
  `VerificationToken` varchar(500) NOT NULL,
  `Role` varchar(50) NOT NULL,
  `RegistrationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SchoolClassID` int DEFAULT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Email` (`Email`),
  KEY `FK_Users_SchoolClasses` (`SchoolClassID`),
  CONSTRAINT `FK_Users_SchoolClasses` FOREIGN KEY (`SchoolClassID`) REFERENCES `schoolclasses` (`SchoolClassID`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Admin','Admin','$2a$11$.QsyJMGqIQ3fl1n7Km17teYqzJH/f0rtwq1v2p3cBRihw6MK5/gIS','Admin',0,'sdawdsawd','Admin','2025-02-25 22:29:52',1,'2025-02-25 22:29:52',NULL,NULL),(2,'Alice','Rossi','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','37552810',0,'sdawdsawd','Student','2025-02-25 22:29:52',1,'2025-02-25 22:29:52',NULL,NULL),(3,'Luca','Bianchi','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','375352810222',0,'sdawdsawd','Student','2025-02-25 22:29:52',1,'2025-02-25 22:29:52',NULL,NULL),(4,'Marco','Neri','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','375528150222',0,'sdawdsawd','Teacher','2025-02-25 22:29:52',NULL,'2025-02-25 22:29:52',NULL,NULL),(5,'Alice','Rossi','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','37552821022',0,'sdawdsawd','MasterStudent','2025-02-25 22:29:52',1,'2025-02-25 22:29:52',NULL,NULL),(6,'Luca','Bianchi','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','37551281022',0,'sdawdsawd','MasterStudent','2025-02-25 22:29:52',3,'2025-02-25 22:29:52',NULL,NULL),(7,'Marco','Verdi','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','37552815022',0,'sdawdsawd','MasterStudent','2025-02-25 22:29:52',2,'2025-02-25 22:29:52',NULL,NULL),(8,'Giulia','Gialli','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','37552281022',0,'sdawdsawd','MasterStudent','2025-02-25 22:29:52',2,'2025-02-25 22:29:52',NULL,NULL),(9,'Paolo','Neri','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','paolo',0,'sdawdsawd','MasterStudent','2025-02-25 22:29:52',3,'2025-02-25 22:29:52',NULL,NULL),(10,'Martina','Blu','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','3755281022',0,'sdawdsawd','MasterStudent','2025-02-25 22:29:52',3,'2025-02-25 22:29:52',NULL,NULL),(11,'Producer','Producer','$2a$11$TxAb9qp0YmMgve2sEvg9sev60mbawCVYYlTeskG4/AY036rE/WW/a','Producer',1,'sdawdsawd','Producer','2025-02-25 22:29:52',3,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wallets`
--

DROP TABLE IF EXISTS `wallets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wallets` (
  `WalletID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `Balance` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`WalletID`),
  KEY `FK_Wallets_Users` (`UserID`),
  CONSTRAINT `FK_Wallets_Users` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wallets`
--

LOCK TABLES `wallets` WRITE;
/*!40000 ALTER TABLE `wallets` DISABLE KEYS */;
INSERT INTO `wallets` VALUES (1,9,50.00,'2025-02-25 22:29:52',NULL,NULL);
/*!40000 ALTER TABLE `wallets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wallettransactions`
--

DROP TABLE IF EXISTS `wallettransactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wallettransactions` (
  `TransactionID` int NOT NULL AUTO_INCREMENT,
  `WalletID` int NOT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `TransactionType` varchar(50) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `TransactionDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`TransactionID`),
  KEY `IDX_WalletTransactions_WalletID` (`WalletID`),
  CONSTRAINT `FK_WalletTransactions_Wallets` FOREIGN KEY (`WalletID`) REFERENCES `wallets` (`WalletID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wallettransactions`
--

LOCK TABLES `wallettransactions` WRITE;
/*!40000 ALTER TABLE `wallettransactions` DISABLE KEYS */;
/*!40000 ALTER TABLE `wallettransactions` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-02-26 13:07:19
