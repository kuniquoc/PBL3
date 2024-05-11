-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th5 04, 2024 lúc 03:07 PM
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
-- Cơ sở dữ liệu: `pbl3`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `blogs`
--

CREATE TABLE `blogs` (
  `blog_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `content` text NOT NULL,
  `post_time` date NOT NULL,
  `blog_view` text CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `destinations`
--

CREATE TABLE `destinations` (
  `destination_id` int(11) NOT NULL,
  `admin_id` int(11) NOT NULL,
  `destination_name` varchar(255) NOT NULL,
  `destination_address` varchar(255) NOT NULL,
  `open_time` time NOT NULL,
  `close_time` time NOT NULL,
  `open_day` enum('monday','tuesday','wednesday','thursday','friday','saturday','sunday') NOT NULL,
  `destination_html` text CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `destination_image_url` text NOT NULL,
  `rating` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `destinations`
--

INSERT INTO `destinations` (`destination_id`, `admin_id`, `destination_name`, `destination_address`, `open_time`, `close_time`, `open_day`, `destination_html`, `destination_image_url`, `rating`) VALUES
(1, 2, 'Ba Na Hills', 'An Son Village, Hoa Ninh Commune, Hoa Vang District, Da Nang City, Vietnam', '08:00:00', '22:00:00', 'monday', '<p>As part of Sun World Amusement Park Group and over 20 km away from Danang downtown, Sun World ba Na Hills is the most significant resort and recreational complex of Vietnam. At the height of 1,487 m from the sea level, Sun World Ba Na Hills is coined the “heaven on earth” owing to its spectacular climate and otherworldly natural landscape. Just travel to Sun World Ba Na Hills and revel yourselves in the typical rotation of four seasons in a single day and numerous festivities, recreations and relaxation and cuisine.</p>', 'https://th.bing.com/th/id/OIP.NFqYvQE4BusNly_wMRJidQHaFj?rs=1&pid=ImgDetMain, https://explorevietnam.com.vn/hoi-an/wp-content/uploads/2019/04/Sunset-at-Golden-Bridge-Ba-Na-Hills.jpg, https://th.bing.com/th/id/R.53af2068a98ce9309975da727b06c2ee?rik=6d270yK87I%2baOw&riu=http%3a%2f%2fvietcetera.com%2fwp-content%2fuploads%2f2016%2f12%2fBana-Hills.jpg&ehk=7dc3T2RF65n9XZzS3tE7J5N4rrfSjahGwKawIufst2M%3d&risl=&pid=ImgRaw&r=0', 4.5),
(2, 2, 'Nui Than Tai Hot Springs Park', '14G Highway, Hoa Phu Commune, Hoa Vang District, Danang City, Vietnam', '07:00:00', '20:00:00', 'monday', '<p>Nui Than Tai not only has the beautiful natural scenery in Ba Na Nui Chua but also owns the unique hot mineral springs. Thanks to the favor which donated by Mother Nature, Nui Than Tai always know how to charm the tourists with the magic charm, etc. Nui Than Tai Hot Springs Park is about 20 km from the center of Da Nang City and it takes visitors about 30 minutes to travel by car. They can visit during the day or overnight stay. Here, visitors can admire and discover the majestic beauty of the mountains as well as be immersed in the fresh air of four seasons nature in a day.</p>', 'https://nuithantai.vn/Content/UserFiles/Images/About/1%20copy.jpg;https://nuithantai.vn/Content/UserFiles/Images/About/Cong%20vien%20nuoc%202.JPG', 4.6);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `favourite_destinations`
--

CREATE TABLE `favourite_destinations` (
  `favourite_destination_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `destination_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `processing_reports`
--

CREATE TABLE `processing_reports` (
  `processing_report_id` int(11) NOT NULL,
  `admin_id` int(11) NOT NULL,
  `report_id` int(11) NOT NULL,
  `content` text NOT NULL,
  `processing_time` datetime NOT NULL,
  `type_processing` enum('Blog','Review','Destination') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `reports`
--

CREATE TABLE `reports` (
  `report_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `invoke_id` int(11) NOT NULL,
  `content` text NOT NULL,
  `report_time` datetime NOT NULL,
  `type_report` enum('destination','blog','review') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `reviews`
--

CREATE TABLE `reviews` (
  `review_id` int(11) NOT NULL,
  `destination_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `content` text NOT NULL,
  `star` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `schedules`
--

CREATE TABLE `schedules` (
  `schedule_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `schedule_name` varchar(255) NOT NULL,
  `schedule_describe` text NOT NULL,
  `status` enum('planning','processing','completed','cancelled') NOT NULL,
  `is_public` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `schedule_destinations`
--

CREATE TABLE `schedule_destinations` (
  `sd_id` int(11) NOT NULL,
  `schedule_id` int(11) NOT NULL,
  `destination_id` int(11) NOT NULL,
  `arrival_time` datetime NOT NULL,
  `leave_time` datetime NOT NULL,
  `cost_estimate` bigint(20) NOT NULL,
  `note` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `users`
--

CREATE TABLE `users` (
  `user_id` int(11) NOT NULL,
  `full_name` varchar(255) NOT NULL,
  `birthday` date NOT NULL,
  `email` varchar(255) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `permission` enum('admin','user') NOT NULL DEFAULT 'user',
  `avatar_url` text CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `users`
--

INSERT INTO `users` (`user_id`, `full_name`, `birthday`, `email`, `user_name`, `password`, `permission`, `avatar_url`) VALUES
(1, 'Nguyễn Văn Huy', '2004-08-15', 'nvhuy@gmail.com', 'NVHuy', '15082004', 'admin', 'string'),
(2, 'Lê Nguyễn Phúc Sinh', '2004-10-18', 'lnpsinh@gmail.com', 'LSinh1810', '18102004', 'admin', 'string'),
(3, 'Nguyễn Đức Bảo', '2004-08-12', 'ndbao@gmail.com', 'NDBao', '12082004', 'admin', 'string'),
(4, 'Lê Xuân Bá Quốc', '2004-07-19', 'lxbquoc@gmail.com', 'LXBQuoc', '19072004', 'admin', 'string'),
(5, 'Lê Văn Mười', '1975-05-19', 'lvmuoi@gmail.com', 'LVMuoi', '19051975', 'user', 'string'),
(6, 'Nguyễn Thị Kim Ngân', '2007-09-12', 'ntkngan@gmail.com', 'NTKNgan', '12092007', 'user', 'string'),
(7, 'Trần Văn Tiến', '2005-10-31', 'tvtien@gmail.con', 'TVTien', '31102005', 'user', 'string'),
(8, 'Võ Bảo Hoàng', '2001-03-20', 'vbhoang@gmail.com', 'VBHoang', '20032001', 'user', 'string'),
(9, 'Nguyễn Ngọc Bảo', '2001-05-17', 'nnbao@gmail.com', 'NNBao', '17052001', 'user', 'string'),
(10, 'Tô Thanh Châu', '1999-02-02', 'ttchau@gmail.com', 'TTChau', '02021999', 'user', 'string');

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
-- Chỉ mục cho bảng `destinations`
--
ALTER TABLE `destinations`
  ADD PRIMARY KEY (`destination_id`),
  ADD KEY `FK_destination_admin` (`admin_id`);

--
-- Chỉ mục cho bảng `favourite_destinations`
--
ALTER TABLE `favourite_destinations`
  ADD PRIMARY KEY (`favourite_destination_id`),
  ADD KEY `FK_fav_destination_user` (`user_id`),
  ADD KEY `FK_fav_destination_dest` (`destination_id`);

--
-- Chỉ mục cho bảng `processing_reports`
--
ALTER TABLE `processing_reports`
  ADD PRIMARY KEY (`processing_report_id`),
  ADD KEY `FK_processing_report_admin` (`admin_id`);

--
-- Chỉ mục cho bảng `reports`
--
ALTER TABLE `reports`
  ADD PRIMARY KEY (`report_id`),
  ADD KEY `FK_report_user` (`user_id`),
  ADD KEY `FK_report_invoke` (`invoke_id`);

--
-- Chỉ mục cho bảng `reviews`
--
ALTER TABLE `reviews`
  ADD PRIMARY KEY (`review_id`),
  ADD KEY `FK_review_destination` (`destination_id`),
  ADD KEY `FK_review_user` (`user_id`);

--
-- Chỉ mục cho bảng `schedules`
--
ALTER TABLE `schedules`
  ADD PRIMARY KEY (`schedule_id`),
  ADD KEY `FK_schedule_user` (`user_id`);

--
-- Chỉ mục cho bảng `schedule_destinations`
--
ALTER TABLE `schedule_destinations`
  ADD PRIMARY KEY (`sd_id`),
  ADD KEY `FK_schedule_dest_schedule` (`schedule_id`),
  ADD KEY `FK_schedule_dest_destination` (`destination_id`);

--
-- Chỉ mục cho bảng `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `blogs`
--
ALTER TABLE `blogs`
  MODIFY `blog_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `destinations`
--
ALTER TABLE `destinations`
  MODIFY `destination_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT cho bảng `favourite_destinations`
--
ALTER TABLE `favourite_destinations`
  MODIFY `favourite_destination_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `processing_reports`
--
ALTER TABLE `processing_reports`
  MODIFY `processing_report_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `reports`
--
ALTER TABLE `reports`
  MODIFY `report_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `reviews`
--
ALTER TABLE `reviews`
  MODIFY `review_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `schedules`
--
ALTER TABLE `schedules`
  MODIFY `schedule_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `schedule_destinations`
--
ALTER TABLE `schedule_destinations`
  MODIFY `sd_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `blogs`
--
ALTER TABLE `blogs`
  ADD CONSTRAINT `FK_blog_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `destinations`
--
ALTER TABLE `destinations`
  ADD CONSTRAINT `FK_destination_admin` FOREIGN KEY (`admin_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `favourite_destinations`
--
ALTER TABLE `favourite_destinations`
  ADD CONSTRAINT `FK_fav_destination_dest` FOREIGN KEY (`destination_id`) REFERENCES `destinations` (`destination_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_fav_destination_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `processing_reports`
--
ALTER TABLE `processing_reports`
  ADD CONSTRAINT `FK_processing_report_admin` FOREIGN KEY (`admin_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `reports`
--
ALTER TABLE `reports`
  ADD CONSTRAINT `FK_report_invoke` FOREIGN KEY (`invoke_id`) REFERENCES `reports` (`report_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_report_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `reviews`
--
ALTER TABLE `reviews`
  ADD CONSTRAINT `FK_des_id_review` FOREIGN KEY (`destination_id`) REFERENCES `destinations` (`destination_id`),
  ADD CONSTRAINT `FK_review_destination` FOREIGN KEY (`destination_id`) REFERENCES `destinations` (`destination_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_review_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_user_id_review` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Các ràng buộc cho bảng `schedules`
--
ALTER TABLE `schedules`
  ADD CONSTRAINT `FK_schedule_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `schedule_destinations`
--
ALTER TABLE `schedule_destinations`
  ADD CONSTRAINT `FK_schedule_dest_destination` FOREIGN KEY (`destination_id`) REFERENCES `destinations` (`destination_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_schedule_dest_schedule` FOREIGN KEY (`schedule_id`) REFERENCES `schedules` (`schedule_id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
