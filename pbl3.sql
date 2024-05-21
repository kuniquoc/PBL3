-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th5 21, 2024 lúc 06:20 PM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.0.30

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
  `password_hash` blob NOT NULL,
  `password_salt` blob NOT NULL,
  `permission` enum('admin','user') NOT NULL DEFAULT 'user',
  `avatar_url` text CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `created_at` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `users`
--

INSERT INTO `users` (`user_id`, `full_name`, `birthday`, `email`, `password_hash`, `password_salt`, `permission`, `avatar_url`, `created_at`) VALUES
(18, 'User6035', '2000-01-01', 'ldblckrs', 0xc2f3b474621f1ef265e98227a834bf39c802239cee1b17530f3686bca7a8eacff5b5b30b4235626dbe6db748fa432840d26586e3882a34cd3f3ece86bdc06caf, 0xa19bfa03c356b7bcc77ed5e99009b0736e2ab05dc6c0595d8e2dc6217b5bd965c110a1f312abb97d23f3f7c06c79557dffcb72fc9a778a1525cbd148f5ccd3e1c429fc997be8aaedde0514cc41c31ed5bdfdefdf46a02a48083a7ca82c4f3be2f64a9aaac2dcd0ebac66dce246594a8dfecbf27273335b438e8663aab1631ec5, 'admin', 'https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png', '2024-05-21'),
(19, 'string', '2000-01-01', 'string', 0xe184013947295c325676c9358a6c5942a65a0550fb4f2f204d057dd7245776d9e7d9d198726e930c430bb2c82bbc050d206c640bf1648ddbc11e35841859f7d7, 0x109c2bf5c03e27ce80a8fff0d9b2aba43018c096b6f742a66ebb20d8388e12721c74d109a39ca4dea891e9463ac336a344d3ab6377777f848d4328616873a743341fde38445a26ec5fc4482ca9452cac339d6e8469e4a02892827b0d62ca1d7d5fd24d041a4c98b40ac1f0ce55744eac60ed97d771c004cc5b3491b093f25b2c, 'user', 'https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png', '2024-05-21'),
(20, 'admin', '2000-01-01', 'admin', 0x3c4554415ab9172f75d64c792b655df3d4ddb57ad480e11cb69eeba7b9e723fadf77bd5849cb860b1cd15333e0a160f61f97109c48757bf09b65db340ccd6c23, 0x990e761771578690809125cbec1033c341d890b973f3888044180dda2c0cf0255de81e7df757d3efb84fe223a1af5b2c7121523f1a9eac4ff92eae12e225c6316731453d41cbe1bbeb81bd7e0240584fa9aa3500eb5c8dc92c03331bab9d359e8900163c3c337183b6fcee387b4c34d7202f0bcf6fb81712ade00d78da05a681, 'admin', 'https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png', '2024-05-21');

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
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

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
