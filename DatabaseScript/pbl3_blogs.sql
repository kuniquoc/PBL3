-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th6 10, 2024 lúc 11:41 AM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `plb3_test`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `blogs`
--

CREATE TABLE `blogs` (
  `blog_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `title` text NOT NULL,
  `type` enum('tips','places') NOT NULL,
  `image` text NOT NULL,
  `introduction` text NOT NULL,
  `created_at` datetime NOT NULL,
  `content` text NOT NULL,
  `views` int(11) NOT NULL,
  `status` enum('pending','published','rejected') NOT NULL DEFAULT 'pending'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `blogs`
--

INSERT INTO `blogs` (`blog_id`, `user_id`, `title`, `type`, `image`, `introduction`, `created_at`, `content`, `views`, `status`) VALUES
(1, 2, 'First time to Ba Na Hills', 'places', 'https://th.bing.com/th/id/OIP.iMKsOZA0D8fbSAZV9zoCGgHaE8?rs=1&pid=ImgDetMain', 'Bana Hills is a place worth visiting when arriving in Danang City', '2024-05-11 00:00:00', 'With beautiful natural scenery and cool climate, visitors can travel to Ba Na Hills in any season of the year. However, there are times that are especially suitable for traveling to Ba Na Hills:\r\n\r\nTime from April to October: This is the summer time of the year. Coming to Ba Na at this time, you will enjoy the 4-season climate in 1 unique day, and enjoy the most complete view of Ba Na Hills without fear of fog blocking your sight.\r\nRead more: What\'s special about Ba Na\'s climate?\r\nFestival time: Ba Na Hills is known as \"the region\'s leading event and festival destination\". Festivals often coincide with holidays and Tet with many unique and new experiences. At this time, Ba Na is splendidly decorated, the atmosphere also becomes more bustling and fun.', 2, 'pending'),
(2, 2, 'The Ultimate Guide to Experiencing Da Nang, Viet Nam', 'tips', 'https://duan-sungroup.com/wp-content/uploads/2022/11/cong-vien-chau-a-asia-park-duoc-cai-tao-vao-nam-2020.jpg', 'This guide will help you experience the best of Da Nang, Vietnam. You should visit the Marble Mountains, five limestone outcrops that are home to pagodas and caves containing Buddhist shrines. The city\'s Dragon Bridge is a symbol of Da Nang, and the nearby Museum of Cham Sculpture displays art from the ancient Cham civilization.', '2024-05-01 00:00:00', 'Located in central Vietnam, the city of Da Nang is a beautiful and rapidly growing city that is becoming one of Southeast Asia’s top tourist destinations. Renowned for its breathtaking landscape, rich culture, and incredible cuisine, it is a destination that should be on every traveler’s list. In this Da Nang, Vietnam Guide, I will touch on some of the many things that make this city incredible.\r\n\r\nFrom the city’s breathtaking beaches to the incredible marble mountains that backdrop this gorgeous city, Da Nang is a city that will take your breath away with its beauty. If that isn’t enough to make you want to visit, the city’s many incredible beaches are great for unwinding and and enjoying the weather. Not to be outdone, the incredibly diverse and delicious cuisine in Da Nang is renowned the world over. These are just some of the many reasons to visit this amazing city.\r\n\r\n\r\n\r\nMarble Mountains in Da Nang, Vietnam\r\n\r\n\r\nAn All You Need Da Nang, Vietnam Guide\r\nIn this guide, I am going to provide you will all of the information that you need to plan a successful trip to see Da Nang. I cover the best times to visit and the best ways to get there so that you can maximize what you can see and do on your trip.\r\n\r\nI also outline all of the top things to see and do while you are there, as well as the best places to stay and eat. With the information in my Da Nang, Vietnam guide in hand, you can be confident that your trip to see one of Vietnam’s most incredible cities will be one that you remember for the rest of your life.\r\n\r\n\r\n\r\nAt a Glance\r\nBefore you start making any travel plans, you will want to make sure to take care of all of the passport, VISA, and immunization requirements for your trip.  In addition, you will want to make sure you have a clear understanding of what languages they speak in Vietnam so that you can plan any translation needs you might have.\r\n\r\nYou will also need to know what currency they use so that you can exchange currency before your trip if necessary. I have included some of this key information in my Da Nang, Vietnam guide below for you to review as you start to make your travel plans.\r\n\r\n\r\n\r\nPassport, VISA, Customs, and Immunization Requirements\r\nPassport\r\n\r\nI have included a link to my Vietnam Passport, VISA, Customs, and Immunization Requirements for Visitors Guide for you to review below. This should help you navigate the legal requirements for visiting Vietnam.\r\n\r\nIt includes all of the important VISA, passport, customs, and immunization requirements and recommendations for your visit. You will want to take special care in reviewing the immunizations section of this guide.\r\n\r\nIn addition to the immunization information in the guide above, I have also linked to a few supplemental health guides I have created below. I explain why I recommend getting the Rabies pre-exposure vaccination before traveling internationally.\r\n\r\nI also discuss how to protect yourself from tick and mosquito-borne illnesses when traveling. Even though Da Nang isn’t in a high-risk area for malaria, it is still good to protect yourself just in case.\r\n\r\n\r\n\r\nPacking Tips\r\nSuitcase for Travel\r\n\r\nOutside of taking care of your passport, visa, and immunization requirements, the most important task for your trip is packing. For your trip to be a success, you need to make sure you pack the right clothing and gear for the weather and the activities you will be enjoying. To help make sure you are prepared, I linked to my packing resources for you to review in my Da Nang, Vietnam guide below.', 0, 'published'),
(5, 0, 'Da Nang Travel Experience 3 Days 2 Nights Self-sufficient From A to Z', 'tips', 'https://res.cloudinary.com/dxhuysuy5/image/upload/v1718010264/dne/ik1jnpqzqxqccnkgw3pq_pnyotl.webp', 'What to do in Da Nang with only 3 days and 2 nights? Check out the super fun, super new and super economical 3 days 2 nights Da Nang travel experience from Klook Vietnam!', '2024-06-10 11:20:12', '<h1>Da Nang 3 Days 2 Nights Tour Experience</h1><h2>Day 1: Suoi Khoang Nong Than Hot Spring Park - My Xuan Boat Cruise on Han River</h2><p><img src=\\\"http://res.cloudinary.com/dxhuysuy5/image/upload/v1718012014/dne/yxoazj88l1hdk2ln9ciu_fyvg7d.webp\\\"></p><p>From Da Nang airport, you use the service to the hotel. After checking in, you can rest, take a walk around the city center or Da Nang beach and enjoy delicious Da Nang food.</p><p>The Da Nang 3-day 2-night tour officially begins with a visit to Suoi Khoang Nong Thain Hot Spring Park. Here, you will enjoy professional health care services, heal wounds and relieve stress of your body in a natural way. In addition, Suoi Khoang Nong Thain Hot Spring Park also has many exciting entertainment activities suitable for the whole family. Just to \\\"fog\\\" it, there is a primeval forest system of tens of hectares, a mud bathing area combined with hydrotherapy, a \\\"standard Japanese\\\" onsen tower and a 9D, 12D movie viewing service \\\"eye-catching\\\". Don\'t forget to visit the God of Wealth Temple to cast a fortune and pray for a year of prosperity and abundance.</p><p>In addition to buying tickets directly, you can also choose a tour to visit Suoi Khoang Nong Thain Hot Spring Park to say goodbye to the worries about transportation and simply enjoy the Da Nang 3-day 2-night tour.</p><p><img src=\\\"http://res.cloudinary.com/dxhuysuy5/image/upload/v1718012028/dne/ayju298jgldlcn6xchqt_top0bp.webp\\\"></p><p>After visiting Suoi Khoang Nong Thain Hot Spring Park, you can end the first day with a romantic evening on the My Xuan cruise with your family and friends. The 45-minute cruise on the Han River will surely bring you many memorable moments, overlooking the whole city sparkling in the lights and the gentle entertainment program. In addition, you will also be served with water and fresh fruit, making the experience even more exciting.</p><h2>Day 2: Ba Na Hills and Golden Bridge Tour - Free Visit</h2><p><img src=\\\"http://res.cloudinary.com/dxhuysuy5/image/upload/v1718012052/dne/imrk0roszlb6dnjelaad_lhqiwk.webp\\\"></p><p>On the second day of the Da Nang 3-day 2-night tour, wake up early to go \\\"flying fairyland\\\" at Ba Na Hills and Golden Bridge tour in the day. From the Ba Na Hills cable car, you will be amazed by the scenery of white clouds floating in the middle of the vast mountains and forests, majestic and poetic like stepping out of a movie.</p><p>Ba Na Hills tourist area is also known as \\\"Europe in the heart of Da Thanh\\\". If you travel to Da Nang for 3 days 2 nights with young children, you will see the excitement of the children when they come to the French Village, enjoy the classic French architecture or take \\\"virtual life\\\" photos in the Le Jardin D\'amour flower garden. From the village, you can easily move to Linh Ung Pagoda, where there is a 27-meter high Buddha statue that is extremely impressive.</p><p>Next, go to Loc Uyen Garden and Quan Am Cac to walk through the carefully trimmed rows of trees, and take impressive photos together. Stop at Debay Station on the top of Nui Chua to enjoy the beauty of the nature reserve before having a meal at Ba Na Hills restaurant with over 100 attractive dishes.</p><p><img src=\\\"http://res.cloudinary.com/dxhuysuy5/image/upload/v1718012069/dne/ih2yjvauifsimepu8oiv_qafyfw.webp\\\"></p><p>Many visitors go to Ba Na Hills just to experience the feeling of crossing heaven on the \\\"most impressive walking bridge in the world\\\" - the Golden Bridge - located at an altitude of 1,400 meters above sea level. With an extremely impressive design like a ribbon held up by two giant hands in mid-air, the Golden Bridge is an ideal place to admire the lush vegetation and the magnificent terrain of the Truong Son Mountains.</p><p>Oh, don\'t forget to visit Fantasy Park. This is truly an entertainment paradise with over 100 exciting games, built and designed inspired by two famous novels by French writer Jules Verne. Let the little members have fun, try the thrill of the roller coaster with their family and create memorable memories. At the final stop, visiting Linh Chua Linh Tu Temple to visit the temple complex to learn about Vietnamese Buddhist culture is not a bad idea at all.</p><p>After a tiring day of sightseeing, you can enjoy a peaceful evening at the hotel, or stroll around the streets to enjoy the specialties of the place, or find a \\\"lemongrass\\\" spa to pamper your body.</p><h2>Day 3: Private Walking Tour in Hoi An from Da Nang + Fun at the Upside Down Museum</h2><p><img src=\\\"http://res.cloudinary.com/dxhuysuy5/image/upload/v1718012089/dne/bbvhmxkc8wr462z2otkw_eb67o1.webp\\\"></p><p>Welcome to Hoi An Ancient Town, on the last day of the Da Nang 3-day 2-night tour. This private tour for 2 people includes transportation from Da Nang city, so you don\'t have to worry about getting around.</p><p>Hoi An Ancient Town welcomes visitors with a small path leading to Phuc Kien Congregation - a temple built in Chinese style. The next stop is Tan Ky\'s ancient house, where you will find the relic of the prosperous trade and cultural exchange between Vietnamese, Chinese and Japanese in the 18th century. Cau Bridge or Lai Vien Kieu - a typical piece with a history of hundreds- it is also a monument worth visiting. This is one of the most famous bridges in Vietnam, built by the Japanese community when approaching the Chinese neighborhoods in Hoi An.</p><p><img src=\\\"http://res.cloudinary.com/dxhuysuy5/image/upload/v1718012218/dne/kf1seonuxlyyvpvpcrf6_jmka7x.webp\\\"></p><p>The tour to Hoi An from Da Nang ends at Phung Hung House - just a few minutes walk from Chua Cau. Those who love art will surely enjoy looking at the two-storey multi-style house, with hints of Chinese and Japanese architecture, plus handicraft products considered as Vietnam\'s cultural heritage.</p><p><br></p><p>The 3-day, 2-arrival self-sufficient travel experience to Da Nang is ready; Why hesitate to reward yourself with an exciting outing? Don\'t forget to browse other Blogs to accumulate more self-sufficiency, lists,... for your upcoming trip filled with new energy.</p><p>Who will you travel to Da Nang for 3 days and 2 nights?</p>', 0, 'pending');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `blogs`
--
ALTER TABLE `blogs`
  ADD PRIMARY KEY (`blog_id`),
  ADD KEY `FK_blog_user` (`user_id`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `blogs`
--
ALTER TABLE `blogs`
  MODIFY `blog_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
