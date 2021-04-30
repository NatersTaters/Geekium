f-- ********************************************************
-- This script creates the Geekium Webstore database
-- for our INFO3220 Capstone project by Nathaniel Saunders, 
-- Andrew Truong and Leunard Gervalla 
-- ********************************************************

-- Create the database
--DROP DATABASE IF EXISTS geekium_db;
--CREATE DATABASE geekium_db;

-- Select the database
USE geekium_db;

-- Create the tables
CREATE TABLE accounts
(
	account_id					INT				IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    user_name					VARCHAR(50)		NOT NULL			UNIQUE,
    user_password 				VARCHAR(50)		NOT NULL,
	pasword_hash				VARCHAR(max)	NOT NULL,
    first_name					VARCHAR(50)		DEFAULT NULL,
    last_name					VARCHAR(50)		DEFAULT NULL,
    email						VARCHAR(100)	NOT NULL,
    point_balance				INT 			DEFAULT NULL
); 

CREATE TABLE sellerAccounts
(
	seller_id					INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    account_id					INT 			NOT NULL,
    average_rating				FLOAT 			DEFAULT NULL,
    CONSTRAINT sellerAccounts_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id)
); 

CREATE TABLE sellerReviews
(
	seller_review_id			INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    seller_id					INT 			NOT NULL,
	account_id					INT				NOT NULL,
    buyer_rating				FLOAT			DEFAULT NULL,
    review_description			VARCHAR(255)	DEFAULT NULL,
    CONSTRAINT sellerReviews_fk_sellerAccounts
		FOREIGN KEY (seller_id)
		REFERENCES sellerAccounts (seller_id),
	CONSTRAINT sellerReviews_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id)
); 

CREATE TABLE rewards
(
	reward_id					INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    account_id					INT      		NOT NULL,
    reward_type					VARCHAR(50)		NOT NULL,
    reward_code					VARCHAR(100)	NOT NULL,
    point_cost					INT 			DEFAULT NULL,
    date_received				DATE			NOT NULL,
    CONSTRAINT rewards_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id)
); 

CREATE TABLE cart
(
	cart_id						INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    account_id					INT 			NOT NULL,
	transactionComplete			BIT				DEFAULT 0,
	number_of_products			INT 			DEFAULT NULL,
    total_price					FLOAT 			DEFAULT NULL,
    points_gained				INT 			DEFAULT NULL,
    CONSTRAINT cart_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id)
);

CREATE TABLE accountPurchases
(
	account_purchase_id			INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    account_id					INT 			NOT NULL,
	cart_id						INT				NOT NULL,
    purchase_date				DATE			NOT NULL,
    purchase_price				FLOAT 			NOT NULL,
    tracking_number				INT 			NOT NULL,
    points_gained				INT      		NOT NULL,
    CONSTRAINT accountPurchases_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id),
	CONSTRAINT accountPurchases_fk_cart
		FOREIGN KEY (cart_id)
		REFERENCES cart (cart_id),
);

CREATE TABLE receipt
(
	receipt_id					INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    cart_id						INT 			NOT NULL,
    CONSTRAINT receipt_fk_cart
		FOREIGN KEY (cart_id)
		REFERENCES cart (cart_id)
);

CREATE TABLE serviceListings
(
	service_listing_id			INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    account_id					INT 			NOT NULL,
	service_title				VARCHAR(100)	NOT NULL,
    service_description			VARCHAR(255)	NOT NULL,
    listing_date				DATE			NOT NULL,
	service_location			VARCHAR(255)	DEFAULT NULL,
	service_image				VARCHAR(255)	DEFAULT NULL,
    CONSTRAINT serviceListings_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id)
);

CREATE TABLE priceTrends
(
    price_trend_id              INT             IDENTITY(1,1)        PRIMARY KEY     NOT NULL,
    item_name                   VARCHAR(100)    DEFAULT NULL,
    date_of_update              DATE            NOT NULL,
    average_price               FLOAT           NOT NULL,
    lowest_price                FLOAT           NOT NULL,
    highest_price               FLOAT           NOT NULL
);

CREATE TABLE sellListings
(
	sell_listing_id				INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    seller_id					INT 			NOT NULL,
	price_trend_id				INT 			NOT NULL,
	sell_title					VARCHAR(100)	NOT NULL,
    sell_description			VARCHAR(255)	NOT NULL,
    sell_price					FLOAT 			NOT NULL,
    sell_date					DATE			NOT NULL,
    sell_item_type				VARCHAR(100)	NOT NULL,
    sell_quantity				INT 			NOT NULL,
	sell_image					VARCHAR(255)	DEFAULT NULL,
	price_trend_keywords		VARCHAR(255)	DEFAULT NULL,
	display						BIT				DEFAULT 1,
    CONSTRAINT sellListings_fk_sellerAccounts
		FOREIGN KEY (seller_id)
		REFERENCES sellerAccounts (seller_id),
	CONSTRAINT sellListings_fk_priceTrends
		FOREIGN KEY (price_trend_id)
        REFERENCES priceTrends (price_trend_id)
);

CREATE TABLE tradeListings
(
	trade_listing_id			INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    seller_id					INT 			NOT NULL,
	trade_title					VARCHAR(100)	NOT NULL,
    trade_description			VARCHAR(255)	NOT NULL,
    trade_for					VARCHAR(100)	NOT NULL,
    trade_date					DATE			NOT NULL,
    trade_item_type				VARCHAR(100)	NOT NULL,
    trade_quantity				INT 			NOT NULL,
	trade_image					VARCHAR(255)	DEFAULT NULL,
	trade_location				VARCHAR(255)	DEFAULT NULL,
	display						BIT				DEFAULT 1,
    CONSTRAINT tradeListings_fk_sellerAccounts
		FOREIGN KEY (seller_id)
		REFERENCES sellerAccounts (seller_id)
);

CREATE TABLE itemsForCart
(
	items_for_cart_id			INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    cart_id						INT 			NOT NULL,
    sell_listing_id				INT 			NOT NULL,
	quantity					INT				DEFAULT 0			NOT NULL,
    CONSTRAINT itemsForCart_fk_cart
		FOREIGN KEY (cart_id)
		REFERENCES cart (cart_id),
	CONSTRAINT itemsForCart_fk_sellListings
		FOREIGN KEY (sell_listing_id)
        REFERENCES sellListings (sell_listing_id)
);
INSERT accounts (user_name, user_password, pasword_hash, email)
	VALUES ('GeekiumAdmin', '79nuMW6ytu44nrhh5JhT8VD6LXQa13aUSbVSobDWr4Q=', 'LRVHEKRL2KW18EDZONAD74X7Y3D4D3Q8', 'geekium1234@gmail.com')
GO

INSERT accounts (user_name, user_password, pasword_hash, first_name, last_name, email, point_balance)
	VALUES ('BullR00', 'l+NPTnoiZkPLvsftBGgWMeJr0oAw1K1rC6bq4FN+Xt8=', 'R67LQK5XZHHQH9DC9S54U2T92BZ4ARIK', 'Randall', 'Bullman', 'bullyboy@gmail.com', 500)
GO

INSERT accounts (user_name, user_password, pasword_hash, first_name, last_name, email, point_balance)
	VALUES ('CanadianBacon99', '/H8b8fsLHguEOlFpvITzLrQcvj7zIadfJ6Tl18ndup8=', '7BTCPV68N1N0VJLRP0VLLZOMQ4GVVHNC', 'Jeff', 'Tranter', 'tranter885@gmail.com', 30)
GO

INSERT accounts (user_name, user_password, pasword_hash, first_name, last_name, email, point_balance)
	VALUES ('L3GF5', 'ZcEv/ud0kGlhm435lvAwp004zzzsy0CT80KiFr6RFWI=', '5H1CLV42FBKNECARXUP6OFFIWGWUPLMG', 'George', 'Orwell', 'george@live.ca', 120)
GO

INSERT accounts (user_name, user_password, pasword_hash, first_name, last_name, email, point_balance)
	VALUES ('TheDeadPixel00', 'IQ3CUMrhVDA4mHKijCrjcbz1pNATO2EbPBV2/zqb8mQ=', 'OL5ORIG353FJOAIO3Y7B1QXZBTQF9JM4', 'Adam', 'Klinger', 'AdamK@outlook.com', 300)
GO

INSERT sellerAccounts (account_id)
	VALUES (1)
GO

INSERT sellerAccounts (account_id)
	VALUES (3)
GO

INSERT sellerAccounts (account_id)
	VALUES (4)
GO

INSERT sellerAccounts (account_id)
	VALUES (5)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Geekium T-Shirt', '2021-03-26', 19.99, 12.99, 24.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Geekium iPhone Case', '2021-03-26', 14.99, 12.99, 19.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Geekium Water Bottle', '2021-03-26', 9.99, 12.99, 19.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('HP Laptop', '2021-03-26', 899.99, 499.99, 1299.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Pip Boy Replica', '2021-03-26', 129.99, 99.99, 199.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Red Dead Redemption Poster', '2021-03-26', 14.99, 9.99, 29.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Super Mario Maker 3DS', '2021-03-26', 39.99, 24.99, 29.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Star Wars Poster', '2021-03-26', 9.99, 4.99, 12.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Nintendo Switch', '2021-03-26', 399.99, 249.99, 429.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Play Station 4', '2021-03-26', 299.99, 129.99, 549.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Legend of Zelda Replica Sword', '2021-03-26', 149.99, 129.99, 249.99)
GO

INSERT priceTrends (item_name, date_of_update, average_price, lowest_price, highest_price)
	VALUES ('Witcher 3 Map', '2021-03-26', 14.99, 9.99, 19.99)
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (1, 1, 'Geekium T-Shirt', 'T-Shirt with the Geekium logo', 19.99, '2020-10-24', 'Merchandise', 999, 'geekiumtshirt.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (1, 2, 'Geekium iPhone Case', 'iPhone case with the Geekium logo', 14.99, '2020-10-24', 'Merchandise', 999, 'geekiumphonecase.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (1, 3, 'Geekium Water Bottle', 'Water bottle with the Geekium logo', 9.99, '2020-10-24', 'Merchandise', 999, 'geekiumwaterbottle.png')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (2, 4, 'HP Laptop', 'HP 27" Envy Laptop Touchscreen', 899.99, '2020-10-24', 'Computers', 1, 'laptop.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (3, 5, 'Pip-boy Model Replica', 'Fallout 76 Edition Model Pip-Boy', 129.99, '2020-10-24', 'Toys', 1, 'pip-boy.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (4, 6, 'Red Dead Redemption Poster', 'Framed Poster for Red Dead Redemption Game', 14.99, '2020-10-24', 'Toys', 1, 'redemtion-poster.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (3, 7, 'Super Mario Maker 3DS', 'Super Mario Maker Game for the 3DS', 39.99, '2020-10-24', 'Video Games', 1, 'mario-game.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (4, 8, 'Star Wars Poster', 'Metal Poster for Star Wars Episode 4', 9.99, '2020-10-24', 'Toys', 1, 'star-wars-poster.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (2, 9, 'Nintendo Switch', 'Nintendo Switch Console with Joycons', 399.99, '2020-10-24', 'Consoles', 1, 'nintendo-switch.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (2, 10, 'Playstation 4', 'Playstation 4 Slim Console with Controller', 299.99, '2020-10-24', 'Consoles', 1, 'ps4.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (3, 11, 'Legend of Zelda Replica Sword', 'Replica Master Sword From Legend of Zelda Series', 149.99, '2020-10-24', 'Toys', 1, 'zelda-sword.jpg')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (4, 12, 'The Witcher 3 Framed Map', 'Framed Poster of the Witcher 3 Map', 14.99, '2020-10-24', 'Toys', 1, 'witcher-map.jpg')
GO

INSERT tradeListings (seller_id, trade_title, trade_description, trade_for, trade_date, trade_item_type, trade_quantity, trade_image)
	VALUES (3, 'Star Wars Hat', 'Flat Brim Cap with Star Wars Artwork', 'A hat or shirt with the Star Trek logo', '2020-10-24', 'Apparel', 1, 'star-wars-hat.jpg')
GO

INSERT tradeListings (seller_id, trade_title, trade_description, trade_for, trade_date, trade_item_type, trade_quantity, trade_image)
	VALUES (2, 'EVGA Power Supply', '750 Watt EVGA Gold Power Supply', 'A 900 Watt Power Supply', '2020-10-24', 'Computers', 1, 'power-supply.jpg')
GO

INSERT tradeListings (seller_id, trade_title, trade_description, trade_for, trade_date, trade_item_type, trade_quantity, trade_image)
	VALUES (4, 'Watchmen Comic Book', 'Limited Edition Print of Watchment Comic', 'the V for Vendetta comic, any edition is fine', '2020-10-24', 'Books', 1, 'watchmen.jpg')
GO
