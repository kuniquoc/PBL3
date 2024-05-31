-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: pbl3
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
-- Table structure for table `blogs`
--

DROP TABLE IF EXISTS `blogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `blogs` (
  `blog_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `title` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `type` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `image` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `introduction` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` datetime NOT NULL,
  `content` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `views` int NOT NULL,
  `status` enum('pending','published','rejected') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'pending',
  PRIMARY KEY (`blog_id`),
  KEY `fk_blog_user_idx` (`user_id`),
  CONSTRAINT `fk_blog_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `blogs`
--

LOCK TABLES `blogs` WRITE;
/*!40000 ALTER TABLE `blogs` DISABLE KEYS */;
INSERT INTO `blogs` VALUES (1,21,'First time to Ba Na Hills','tips','https://th.bing.com/th/id/OIP.iMKsOZA0D8fbSAZV9zoCGgHaE8?rs=1&pid=ImgDetMain','Bana Hills is a place worth visiting when arriving in Danang City','2024-05-11 00:00:00','With beautiful natural scenery and cool climate, visitors can travel to Ba Na Hills in any season of the year. However, there are times that are especially suitable for traveling to Ba Na Hills:\r\n\r\nTime from April to October: This is the summer time of the year. Coming to Ba Na at this time, you will enjoy the 4-season climate in 1 unique day, and enjoy the most complete view of Ba Na Hills without fear of fog blocking your sight.\r\nRead more: What\'s special about Ba Na\'s climate?\r\nFestival time: Ba Na Hills is known as \"the region\'s leading event and festival destination\". Festivals often coincide with holidays and Tet with many unique and new experiences. At this time, Ba Na is splendidly decorated, the atmosphere also becomes more bustling and fun.',2,'pending'),(2,21,'The Ultimate Guide to Experiencing Da Nang, Viet Nam','All','https://duan-sungroup.com/wp-content/uploads/2022/11/cong-vien-chau-a-asia-park-duoc-cai-tao-vao-nam-2020.jpg','This guide will help you experience the best of Da Nang, Vietnam. You should visit the Marble Mountains, five limestone outcrops that are home to pagodas and caves containing Buddhist shrines. The city\'s Dragon Bridge is a symbol of Da Nang, and the nearby Museum of Cham Sculpture displays art from the ancient Cham civilization.','2024-05-01 00:00:00','Located in central Vietnam, the city of Da Nang is a beautiful and rapidly growing city that is becoming one of Southeast Asia’s top tourist destinations. Renowned for its breathtaking landscape, rich culture, and incredible cuisine, it is a destination that should be on every traveler’s list. In this Da Nang, Vietnam Guide, I will touch on some of the many things that make this city incredible.\r\n\r\nFrom the city’s breathtaking beaches to the incredible marble mountains that backdrop this gorgeous city, Da Nang is a city that will take your breath away with its beauty. If that isn’t enough to make you want to visit, the city’s many incredible beaches are great for unwinding and and enjoying the weather. Not to be outdone, the incredibly diverse and delicious cuisine in Da Nang is renowned the world over. These are just some of the many reasons to visit this amazing city.\r\n\r\n\r\n\r\nMarble Mountains in Da Nang, Vietnam\r\n\r\n\r\nAn All You Need Da Nang, Vietnam Guide\r\nIn this guide, I am going to provide you will all of the information that you need to plan a successful trip to see Da Nang. I cover the best times to visit and the best ways to get there so that you can maximize what you can see and do on your trip.\r\n\r\nI also outline all of the top things to see and do while you are there, as well as the best places to stay and eat. With the information in my Da Nang, Vietnam guide in hand, you can be confident that your trip to see one of Vietnam’s most incredible cities will be one that you remember for the rest of your life.\r\n\r\n\r\n\r\nAt a Glance\r\nBefore you start making any travel plans, you will want to make sure to take care of all of the passport, VISA, and immunization requirements for your trip.  In addition, you will want to make sure you have a clear understanding of what languages they speak in Vietnam so that you can plan any translation needs you might have.\r\n\r\nYou will also need to know what currency they use so that you can exchange currency before your trip if necessary. I have included some of this key information in my Da Nang, Vietnam guide below for you to review as you start to make your travel plans.\r\n\r\n\r\n\r\nPassport, VISA, Customs, and Immunization Requirements\r\nPassport\r\n\r\nI have included a link to my Vietnam Passport, VISA, Customs, and Immunization Requirements for Visitors Guide for you to review below. This should help you navigate the legal requirements for visiting Vietnam.\r\n\r\nIt includes all of the important VISA, passport, customs, and immunization requirements and recommendations for your visit. You will want to take special care in reviewing the immunizations section of this guide.\r\n\r\nIn addition to the immunization information in the guide above, I have also linked to a few supplemental health guides I have created below. I explain why I recommend getting the Rabies pre-exposure vaccination before traveling internationally.\r\n\r\nI also discuss how to protect yourself from tick and mosquito-borne illnesses when traveling. Even though Da Nang isn’t in a high-risk area for malaria, it is still good to protect yourself just in case.\r\n\r\n\r\n\r\nPacking Tips\r\nSuitcase for Travel\r\n\r\nOutside of taking care of your passport, visa, and immunization requirements, the most important task for your trip is packing. For your trip to be a success, you need to make sure you pack the right clothing and gear for the weather and the activities you will be enjoying. To help make sure you are prepared, I linked to my packing resources for you to review in my Da Nang, Vietnam guide below.',0,'published');
/*!40000 ALTER TABLE `blogs` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-05-31 13:22:50
