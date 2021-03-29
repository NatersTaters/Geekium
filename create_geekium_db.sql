-- ********************************************************
-- This script creates the Geekium Webstore database
-- for our INFO3220 Capstone project by Nathaniel Saunders, 
-- Andrew Truong and Leunard Gervalla 
-- ********************************************************

-- Create the database
-- DROP DATABASE IF EXISTS geekium_db;
-- CREATE DATABASE geekium_db;

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
    average_rating				INT 			DEFAULT NULL,
    CONSTRAINT sellerAccounts_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id)
); 

CREATE TABLE sellerReviews
(
	seller_review_id			INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    seller_id					INT 			NOT NULL,
    buyer_rating				INT 			DEFAULT NULL,
    review_description			VARCHAR(255)	DEFAULT NULL,
    CONSTRAINT sellerReviews_fk_sellerAccounts
		FOREIGN KEY (seller_id)
		REFERENCES sellerAccounts (seller_id)
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
    total_price					INT 			DEFAULT NULL,
    points_gained				INT 			DEFAULT NULL,
    CONSTRAINT cart_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id)
);

CREATE TABLE accountPurchases
(
	account_purchase_id			INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    account_id					INT 			NOT NULL,
    seller_id					INT 			NOT NULL,
	cart_id						INT				NOT NULL,
    purchase_date				DATE			NOT NULL,
    purchase_price				INT 			NOT NULL,
    tracking_number				INT 			NOT NULL,
    seller_name					VARCHAR(100)	NOT NULL,
    points_gained				INT      		NOT NULL,
    CONSTRAINT accountPurchases_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id),
	CONSTRAINT accountPurchases_fk_sellerAccounts
		FOREIGN KEY (seller_id)
		REFERENCES sellerAccounts (seller_id),
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
	service_image				VARCHAR(255)	DEFAULT NULL,
    CONSTRAINT serviceListings_fk_accounts
		FOREIGN KEY (account_id)
		REFERENCES accounts (account_id)
);

CREATE TABLE priceTrends
(
	price_trend_id				INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    item_name					VARCHAR(100)	DEFAULT NULL,
    date_of_update				DATE			NOT NULL,
    average_price				INT 			DEFAULT NULL,
    lowest_price				INT 			DEFAULT NULL,
    highest_price				INT 			DEFAULT NULL
);

CREATE TABLE sellListings
(
	sell_listing_id				INT 			IDENTITY(1,1)		PRIMARY KEY     NOT NULL,
    seller_id					INT 			NOT NULL,
    price_trend_id				INT 			NOT NULL,
	sell_title					VARCHAR(100)	NOT NULL,
    sell_description			VARCHAR(255)	NOT NULL,
    sell_price					INT 			NOT NULL,
    sell_date					DATE			NOT NULL,
    sell_item_type				VARCHAR(100)	NOT NULL,
    sell_quantity				INT 			NOT NULL,
	sell_image					VARCHAR(255)	DEFAULT NULL,
	price_trend_keywords		VARCHAR(255)	DEFAULT NULL,
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
	VALUES ('GeekiumAdmin', '79nuMW6ytu44nrhh5JhT8VD6LXQa13aUSbVSobDWr4Q=', 'LRVHEKRL2KW18EDZONAD74X7Y3D4D3Q8', 'admin@email.com')
GO

INSERT sellerAccounts (account_id)
	VALUES (1)
GO

INSERT priceTrends (date_of_update)
	VALUES ('2021-03-26')
GO

INSERT sellListings (seller_id, price_trend_id, sell_title, sell_description, sell_price, sell_date, sell_item_type, sell_quantity, sell_image)
	VALUES (1, 1, 'Geekium T-Shirt', 'T-Shirt with the Geekium logo', 19.99, '2020-10-24', 'Merchandise', 999, '~/images/geekiumtshirt.jpg')
GO