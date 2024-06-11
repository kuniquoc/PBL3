-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th6 11, 2024 lúc 08:30 PM
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
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `users`
--

INSERT INTO `users` (`user_id`, `full_name`, `birthday`, `email`, `password_hash`, `password_salt`, `permission`, `avatar_url`, `created_at`) VALUES
(90000001, 'Huy Hai Thanh', '2004-02-07', 'huyhaithanh51@gmail.com', 0x3ac7b92c1bc9765acb88a85bc6a0887c47394d9dbe346aa838abc23cbfcbb8dc841b600d7fef0da1d22b801a459cebd5b2bd8dcf806223d5b20ce9106d14b37f, 0xedc6b5d34b6be155250c160b0ff02331ce2a33e6a528e11dec5099c8d576d14e63715aae0abc228a392c3491edad46f812c6c7de60e84ca5fec3fd69e183ba12dd63e9b407e0cd8c80e0cc62f8900977a3e7d539a90a4f9faf232f6c739a3d0d481f9168a47febeea3ffc44326064c51ca533306b214ddda435eb5a92ea876c3, 'admin', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718115886/dne/1_xsquom.jpg', '2024-06-11 16:23:17'),
(90000002, 'Ba Quoc', '2004-01-01', 'lxbquoc@gmail.com', 0xe5d249166481afc3b133ea387598b245aa0a155b7f89a165940f40a94209bbd2699bb507779ff272a12991b39dcd284c263c94ffe81e5b8d946fbcc85ab19eb3, 0x97f82822bc46e5dff323a967729bfc0a8d44c86353b6f2d5040a32470c63f86e7be0ef5207fddddc4b7ebb909ab8a4390d311d4d96b7487c1c7d69ce367c225f74dd8b391f513ee5f97b3556f494173021febcbf4b6eff77da4eff25b8abc54642a0a83f76ce3187050c5da7ab214b412f256379b01ca14e9d13ecbac3b35d91, 'admin', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127264/dne/9797e768-1e05-49f0-ab9f-1d5fce9ce290_gp01zo.jpg', '2024-06-11 16:24:59'),
(90000003, 'Nguyen Van An', '1970-01-01', 'user001@gmail.com', 0xe79e7bf16232af3e1e7254498c76956b2b98f2326dbc047fc1fd53731fe2a51b67bd3e9441a57d5d82efcf4a76aabd346a2f85ac956267d2719667697ad6e3d4, 0x39cd3c6a7e4abe7fd81bd24984bd4b09a0474e71bf99f863f53e9f8b06ed6fa7d884d270454140de65a8a5e6be99bcf54c6189a4f2608f89a29178090a7c899e057e25a73a59188dcb2e97237ff6151bb2faca4ff32bfc9b4da3ca8d5705c1094cc749958a9e27f4cb51f56b22eaa4dab52913b49625ed840d087755d86c001f, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127392/dne/0b8b3b85-7888-4f2e-adcb-e77d98bce39a_d6sqce.jpg', '2024-06-11 16:26:21'),
(90000004, 'Tran Van Quyet', '1970-01-01', 'user002@gmail.com', 0x473cac293b5a9c4c3644dc01a48622593e29e5c63b4a5da955fe60fb4545eee693988cb372804dacc2c2cbd21580cb1b55ca294ee6aa9973fa4182f2738e1123, 0xc21bfa6a3a6d818e04f0a1a5ef5fc845079bb2ac017692bc0867fd5c204295b3571a0efc9e5a60c15184dcb607bcbac8f256f743123b2eddfd4adb7b0b3a5285f66f92e4d83d92349a4e49c6bcaa1a710a7c8707f02b2eaf2d1232235639f0223a143e2edefa5c43e674db1ffe4432f461a49208d1c46da49aedc5a6a192bed4, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127420/dne/2a587449-382b-4b0d-856d-e37196d26f6f_iqhoeq.jpg', '2024-06-11 16:26:46'),
(90000005, 'Duc Bao', '2004-06-23', 'ndbao@gmail.com', 0xe9a1a10e4d1313ad80b9e0442cd61e1384f51bb2166cf23118072f056f60676b817d2876947b8cb04b13772e126074d2f545daa248b91ba4916486cd634649bb, 0x9bace60cbe5587565a63a6ccd5b0d700a76cec5a91bfdc9ab7b0902ab1c8cbddd21e6a67497851ad2ea7b2b529a1060ace0a63056f4db0a29fe5124e9409a1d5084a8c67d0618a6ef898e0b8938cb5362aa94080fbefe01baa031a3cdea471fd37efc13a1f60509455e8fdfe27979d7016ef3ba365cc6d86294b4f79aaea99a4, 'admin', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127350/dne/download_b4itlt.jpg', '2024-06-11 16:27:24'),
(90000006, 'Le Sinh', '2004-10-18', 'leesin17531@gmail.com', 0x88df9311436c90bf8ac5522da597ca326dc127854f986f1fa3c1793c4e79e395b1fe5ac115b87b513cc5a114815692aabacb13979ac174e47ce10c428c9193b6, 0x3dc4c450b8cbddb90d680c4b8e08df5ad07b1c751ba4f6e9c124331458f53926977ca2b115f05dd69ad88351dcd0fe2919e4a79da6e3be307fd3f3bb08efa4d29a2a3a5e3c9a21054da99df2c57f6f677c795929ba6aed01bf303b8b0fd6e7f465bce8ac8c7626c6bd24747d999f1d978761f4be67903792802d8b51d09745ea, 'admin', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127233/dne/425673ee-3ed9-4248-b8af-61eaf6ec3965_yfbvan.jpg', '2024-06-11 16:12:33'),
(90000007, 'Tran Dinh Trong', '1970-01-01', 'user003@gmail.com', 0x5707e1f45f27ff1363b03941a9287fc4cf8f9111ff4f7a6e184b3e6dc26d357d8748b4cbfe7a85b653fc19400971cc5619aacb589ec0815612835feb513657cb, 0xafe495f4d368625c57f3d1b0402bce2f5614c0e9ec12288bcb9dfc0b9a591216ba1538fefd3e6b0e0e9f5c462181e609d014fc9c54ef75851dcd9089a8c12f75a5ce1fc94e4f24af1e90f55cbc342537b34096290de4ebb935bfcda83e2c6368092fdd707909e4a0d4f88514f7fbfa1465a1cffb02dd3f7eecd45456dc543130, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127445/dne/2c16467a-e800-4d7c-8007-444bac6e6155_yxjddr.jpg', '2024-06-11 16:35:07'),
(90000008, 'Le Thi Ngoc', '1970-01-01', 'user004@gmail.com', 0x363f27a7297fba18ae88b78972336910573a207a78d7e20244fd0692afb408e88ffb348c0e415e8a71ab9ad70acf266b33517b69df2bebd7bc8e0884353f2968, 0x295f07aff7f02a7c24cf768143b0fbe5d1701f647816887ca7d92c96bc069e2bd653b5403c9805f32657930efaf89cddaad23d8b6e53f7f7933b5c3fa279ac4069179e777e381fe17df2fe3ef0bbb6d9bb98c45d552870d517613996f3ae38d4ea8dfef6f732dce01f49e6d96730d30a125ff53335e2e7cd912ae4d76abfce12, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127476/dne/918a2e68-02a4-4d71-b530-9a42826b1e91_pq4luh.jpg', '2024-06-11 16:37:55'),
(90000009, 'Omen Kazashi', '1970-01-01', 'user005@gmail.com', 0xa0a805d5a348708e031ad8529c4fa4a0dc71a8105dea87c48916d835c9f34c17943858b8dd5b5e3fc66fe8ddc22e1416afdb713015effc6ba049a92baae79b83, 0x43efeb2781d459d07d6b598caf8f551cd90992d4799ab2ef8051604a84341a76d29834890dcca8ede8ebb490f5e389ad653a39241fe673221fbf3ed534d64c6b3a9cc5e81cc849fb0db35ce09d424737ed07093169bb4958ae9b8d7f33d94b84ab6adaa0e0155da8a233b6acb3dfe1de13c3fd9baa7371f7616881cc1f2aaee3, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127511/dne/5f954f1c-75cf-45c5-b0ae-d6f4eebeac38_sv3fnb.jpg', '2024-06-11 16:38:14'),
(90000010, 'Black Cat', '1970-01-01', 'user006@gmail.com', 0xe2bac4d44a9b1551a19ede57e196d2b57f5159e22738047da0ff9d14d1931fdcb00f160c4bba28d76c3315d38304131fd616ad070593052099f1dc271736e095, 0x6d36f3d889957c83e6d7aab1bb42a493f7a5df71bbb39a34e2f30a51e6c7a368d085ff5a227f7cce298784b3db4e19de1f037694cd01b7ceec2e8a6182813e9688d2e9882d8b473a81a8b7edabb00cfa925c1c6a8020e2601938c7108a7270d4b51584ba22c44a144d310a82c5ba6053e80e536c8d62ca91ac47506c6023a964, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127534/dne/17f265aa-0b6e-45f6-aa9c-3de9ec5e9534_eh0sdq.jpg', '2024-06-11 16:38:35'),
(90000011, 'Jing Yuan', '1970-01-01', 'user007@gmail.com', 0x9f38990110a859de5d72716fb0df8837de172a14db5698c55e124fe41357668356701338fe2f450985fa8563e42c8fd503b4e00b31c4dc7ad912558ad9c00c31, 0x8a0e4746b9ba36362166f356fcfcefb81eaeae60ddbcf04e2a7be086e35a31c53f2f3cef0279378e5d1c012b4c7dc89c5e7c8f3a0dd47c34d2707b4c1bcf0fe4549007ccef57f96cc6db7c9eb31478d17e53d9952be3e13188d31760407bf1a76ec3db674748a5bc33586a09c66552c70a421e95a45b7ee525599998f73a1d83, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127563/dne/96ae12a6-f793-4e29-bee2-6b6cabe597ca_zukwcv.jpg', '2024-06-11 16:39:06'),
(90000012, 'Alien Hah', '1970-01-01', 'user008@gmail.com', 0xa440d5c0a0c2c231aa6de5bc4db01ebeae943e756f457ce02b51a2e6383b8de42d027e1c92af2ca0456adfe9572b5ce54632bf4a58930687389555f830b96da8, 0x832d4f46072ca828bbf1f08ffc0fcc4c03efc9497a19f36c184ba46c92de87b696bb7ba924f74123d5c2e4dbe8a1e32a1e86af62c7f3c6a4ab681f9bd6ddd6f828404eccd58003ecb1974d87aecab5d6d72d8006c984ad3c60afcace37dd2d2b093701955383a3d99018a9793e0f8ce45b4a47b28248d9484b61826d9231effb, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127616/dne/171969b3-3bee-4262-8acc-1afe82e495b4_wzns0x.jpg', '2024-06-11 16:39:22'),
(90000013, 'Nguyen Nhat Tam', '1970-01-01', 'user009@gmail.com', 0x97ff8f7f1d7f2bbcafe2410282f6daafce472d0bbb593d124729e6916840d4dc7e150980110208d807ccdc95d4e778bf5181587bf9b96175e56e605b72877d39, 0x27e2738d656202fd70768fd177c1c7b8d351e8fa7efb7857e67479522fb1bcf582665d8e86b2d06ec779e65217f9c71166ad35aa49b4bdd81cfe483f1eb69653cb54f54a8c65b2237364e0036936e2e9ed39fc21d251df11e366684172fd6bd6f93e30afc3e3f2ab6256f15547b654a9eeffeb6225412db5559f6d1183bcc762, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127649/dne/551e94a7-b924-4401-8c6a-5a76cd58e131_t3m11i.jpg', '2024-06-11 16:39:41'),
(90000014, 'Slime King', '1970-01-01', 'user010@gmail.com', 0x91bb41d6f0c6b32929a93fbd3d109d1f906f9a46b4de272f966bbaa0db402afc3799c35d1ed7eded69d0339257734f0abe9f06dcecf35acfbb130433ce00789c, 0x4e8cf6ff3b34586934b0f73265b85b73c509ac22b0238b0d1c27abfcc1f7b018a470f4e53bdc3620ddb19fa777a97ecde1f40ae574b0b2aeb10ec86cc274f56c0dd21ccfc5cd29621f27cfb090d4c03ba2a4d01324bc29774b100f592a9c7644906b77e164c521a68c6fd8f4c19059724817daabb0fabc111318902c68ce3038, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127676/dne/58e24e7b-257c-4329-b64f-1c06ff69952d_hezljv.jpg', '2024-06-11 16:40:32'),
(90000015, '7th Prince', '1970-01-01', 'user011@gmail.com', 0x2649734511a3a4f861d4e578957fdb15af2c5ef753299985485effcd306e12c08aa2a9b5776a6a1df9ad95f678478c8fee277f9e0664aa7ef2dc585733a13c6c, 0x9f01385135fa6b5a7b4a8d622ad44479cd125b4cfa20d265068ad36a47bdfd2e7cfe69942b073ed625f44464994eaced0d7eea654c08fb44ec607b49679675f2836cc8bd7791d0c614058941b13066f1b210cff5148e8641180dfa088c6654d4a302bf06992553389d1d97eb5571d4d9726fea5213778245af438b0c8c98df40, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127701/dne/af69c108-d4cb-4106-a980-be4dbd9d9a0f_iaqgwc.jpg', '2024-06-11 16:40:49'),
(90000016, 'Le Cuong', '1970-01-01', 'user012@gmail.com', 0x36ea4cbc2a7aec06a2338ee1129fc343228a7e4a3680aa557ed0e7b1e9d3ac894a73270759da1a39d06fa7264d88c18a11f10a5482e2dccd2894728312866091, 0x51d5a60caf61a29549dccfc15a50545fdaaef74e341853b466d74fae8e5cb3c3d05b7f91fc9ed90192828021e609649a58ed1c61272201535835f959c2538b27596ebb91a7949ed1b3402be8fd57c9bf25935848446e24609e1152fe1c31651aea5bc6b18425a03d29c8730a0a66e463cb5d46919fce3ace4f1637414b53278c, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127755/dne/b26ee65f-95c4-4106-87d2-6fc9fe701d86_s026jf.jpg', '2024-06-11 16:41:04'),
(90000017, 'Firefly-Sam', '1970-01-01', 'user013@gmail.com', 0xba87b351caee09ed5d347e4a7f0493e2f6410d83a72f27648d396a88b40961965ab08fb543f788385e83c6643e4a2f9d5c3f1b29be1af2544761a4c81474da22, 0xe7c8af19c9918e6627a2e598023259423b8bbfcb13f50f1fead01063ff6d1afd9869936f901aa8f7c7fb6f03a838fb4b1dfad90931679d756af7645dc1454e179437008eb650d9d02f365f151c33462d261f1f3ae7b3efa213e0f2ab8272bf3c5d56da13865bfe98ebf6dbc19247467d74452a833913185aec8a1c6e3f38edeb, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127815/dne/d5844479-d9db-4018-87d3-a8ff9679f0b7_e1leva.jpg', '2024-06-11 16:41:23'),
(90000018, 'Yae Miko', '1970-01-01', 'user014@gmail.com', 0xc7459c55b1acebd8071a9a7e18d7dc84be49cd2cc4511663601a10017ed73fe0092047360e9944741bf4d403ee4f5f5a203f09537b08fdb2ca81516a18322615, 0x96819594fc7cbd5471cc405991f0e10a912cd4b6b811b9480402cbc99352f5d62feace924ed646cb546ad96191d0611e7bc1747a9882153e913ee902d89ec1f665b37cbbfd4164dbb4580796e3c42063c16aa9c9d0f1d413a0d006e11cc9c1dd860a2b6b0a45f64ceaf8a52de1be2999909e558e6e19bc619e2f6b65d718bf6d, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127847/dne/d20528f9-c3f0-4eb1-9562-03c5f47b9e5b_mivhn3.jpg', '2024-06-11 16:41:44'),
(90000019, 'Wubbaboo', '1970-01-01', 'user015@gmail.com', 0x3b1d0c0bb0e0f810065e7277b8edcdfa9c72faff8fcdf0d0bf692fdf3d8d62d820b924f6248925c5ecbef5ab54f0a6d7c8f8fde6acefd8cbd35eaae4cfd675dc, 0xb758973188187fa55391ad861d8b65a0c673e6c9f279d52d3dd63c4c2c8a06e1370c36b7c428ccf72e03753bea2f85f26e323b765a9ca962647d21ddbbfa3c5421c603a1e464b98e45c25bddc52a81f75de89444f25f9a630c6e7553f6ca327206e917a1bc01cb4cc091f912625b5b8b1967fb84b82ab46d2ab280050d64ec70, 'user', 'http://res.cloudinary.com/dxhuysuy5/image/upload/v1718127945/dne/fd4fafe0-da49-4818-8f0f-f71954ae12c0_qgjbss.jpg', '2024-06-11 16:42:04');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=90000020;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
