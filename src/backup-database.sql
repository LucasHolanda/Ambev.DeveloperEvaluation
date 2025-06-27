--
-- PostgreSQL database cluster dump
--

-- Started on 2025-06-27 04:31:30

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE developer;
ALTER ROLE developer WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS;

--
-- User Configurations
--






--
-- Databases
--

--
-- Database "template1" dump
--

\connect template1

--
-- PostgreSQL database dump
--

-- Dumped from database version 13.21 (Debian 13.21-1.pgdg120+1)
-- Dumped by pg_dump version 17.5

-- Started on 2025-06-27 04:31:30

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: developer
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO developer;

--
-- TOC entry 3005 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: developer
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2025-06-27 04:31:30

--
-- PostgreSQL database dump complete
--

--
-- Database "developer_evaluation" dump
--

--
-- PostgreSQL database dump
--

-- Dumped from database version 13.21 (Debian 13.21-1.pgdg120+1)
-- Dumped by pg_dump version 17.5

-- Started on 2025-06-27 04:31:30

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3122 (class 1262 OID 16384)
-- Name: developer_evaluation; Type: DATABASE; Schema: -; Owner: developer
--

CREATE DATABASE developer_evaluation WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE developer_evaluation OWNER TO developer;

\connect developer_evaluation

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: developer
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO developer;

--
-- TOC entry 2 (class 3079 OID 16466)
-- Name: pgcrypto; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA public;


--
-- TOC entry 3124 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION pgcrypto; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pgcrypto IS 'cryptographic functions';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 205 (class 1259 OID 16425)
-- Name: CartProducts; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public."CartProducts" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "CartId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedBy" text,
    "UpdatedBy" text
);


ALTER TABLE public."CartProducts" OWNER TO developer;

--
-- TOC entry 202 (class 1259 OID 16390)
-- Name: Carts; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public."Carts" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "UserId" uuid NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now(),
    "CreatedBy" text DEFAULT 'admin'::text,
    "UpdatedBy" text DEFAULT 'admin'::text,
    "BranchId" integer
);


ALTER TABLE public."Carts" OWNER TO developer;

--
-- TOC entry 203 (class 1259 OID 16398)
-- Name: Products; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public."Products" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Title" character varying(500) NOT NULL,
    "Price" numeric(18,2) NOT NULL,
    "Description" character varying(2000) NOT NULL,
    "Category" character varying(100) NOT NULL,
    "Image" character varying(1000) NOT NULL,
    "Rating_Rate" numeric(3,2) NOT NULL,
    "Rating_Count" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedBy" character varying(100),
    "UpdatedBy" character varying(100)
);


ALTER TABLE public."Products" OWNER TO developer;

--
-- TOC entry 207 (class 1259 OID 16524)
-- Name: SaleItems; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public."SaleItems" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "SaleId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "ProductName" character varying(500) NOT NULL,
    "Quantity" integer NOT NULL,
    "UnitPrice" numeric(18,2) NOT NULL,
    "DiscountPercentage" numeric(5,2) NOT NULL,
    "TotalAmount" numeric(18,2) NOT NULL,
    "CancelationReason" character varying(500),
    "CancelationDate" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedBy" text,
    "UpdatedBy" text,
    "IsCancelled" boolean DEFAULT false
);


ALTER TABLE public."SaleItems" OWNER TO developer;

--
-- TOC entry 206 (class 1259 OID 16514)
-- Name: Sales; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public."Sales" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "SaleNumber" character varying(50) NOT NULL,
    "SaleDate" timestamp with time zone NOT NULL,
    "CustomerId" uuid NOT NULL,
    "TotalAmount" numeric(18,2) NOT NULL,
    "CancelationReason" character varying(500),
    "CancelationDate" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedBy" text,
    "UpdatedBy" text,
    "CartId" uuid,
    "IsCancelled" boolean DEFAULT false
);


ALTER TABLE public."Sales" OWNER TO developer;

--
-- TOC entry 204 (class 1259 OID 16414)
-- Name: Users; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public."Users" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Username" character varying(50) NOT NULL,
    "Email" character varying(100) NOT NULL,
    "Phone" character varying(20) NOT NULL,
    "Password" character varying(100) NOT NULL,
    "Role" character varying(20) NOT NULL,
    "Status" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    "CreatedBy" character varying(100),
    "UpdatedBy" character varying(100)
);


ALTER TABLE public."Users" OWNER TO developer;

--
-- TOC entry 201 (class 1259 OID 16385)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO developer;

--
-- TOC entry 3114 (class 0 OID 16425)
-- Dependencies: 205
-- Data for Name: CartProducts; Type: TABLE DATA; Schema: public; Owner: developer
--

COPY public."CartProducts" ("Id", "CartId", "ProductId", "Quantity", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy") FROM stdin;
\.


--
-- TOC entry 3111 (class 0 OID 16390)
-- Dependencies: 202
-- Data for Name: Carts; Type: TABLE DATA; Schema: public; Owner: developer
--

COPY public."Carts" ("Id", "UserId", "Date", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "BranchId") FROM stdin;
\.


--
-- TOC entry 3112 (class 0 OID 16398)
-- Dependencies: 203
-- Data for Name: Products; Type: TABLE DATA; Schema: public; Owner: developer
--

COPY public."Products" ("Id", "Title", "Price", "Description", "Category", "Image", "Rating_Rate", "Rating_Count", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy") FROM stdin;
df3c7ac2-7c8b-4140-9921-5fcb6295788b	Guaraná Antarctica	6.99	Refrigerante tradicional brasileiro	Bebidas	https://cdn-icons-png.flaticon.com/512/1311/1311095.png	4.70	200	2025-06-23 03:07:34.87493+00	\N	seed	\N
bab639f7-2399-466e-96a4-c1820b27f3b6	Água Mineral	4.50	Água mineral sem gás	Bebidas	https://cdn-icons-png.flaticon.com/512/1311/1311095.png	4.20	80	2025-06-23 03:07:34.87493+00	\N	seed	\N
1456eadf-e952-4586-9495-88892cc9e4d5	Kit Kat	6.99	Chocolate Kit Kat	Doces	https://cdn-icons-png.flaticon.com/512/1311/1311095.png	5.00	2	2025-06-27 01:12:29.114+00	2025-06-27 01:13:45.314912+00	Admin	\N
193f9bb6-b296-4448-abf1-6bf295bd4d76	Suflair	10.00	Chocolate Suflair ao Leite	Frios	https://cdn-icons-png.flaticon.com/512/1311/1311095.png	5.00	2	2025-06-27 01:12:29.114+00	2025-06-27 01:13:45.315259+00	Admin	\N
5d6b449a-64f9-4fe9-8b77-36ef8fe0b9c0	Cerveja Ambev Pilsen	5.99	Cerveja leve e refrescante	Bebidas	https://cdn-icons-png.flaticon.com/512/1311/1311095.png	4.50	120	2025-06-23 03:07:34.87493+00	\N	seed	\N
\.


--
-- TOC entry 3116 (class 0 OID 16524)
-- Dependencies: 207
-- Data for Name: SaleItems; Type: TABLE DATA; Schema: public; Owner: developer
--

COPY public."SaleItems" ("Id", "SaleId", "ProductId", "ProductName", "Quantity", "UnitPrice", "DiscountPercentage", "TotalAmount", "CancelationReason", "CancelationDate", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "IsCancelled") FROM stdin;
\.


--
-- TOC entry 3115 (class 0 OID 16514)
-- Dependencies: 206
-- Data for Name: Sales; Type: TABLE DATA; Schema: public; Owner: developer
--

COPY public."Sales" ("Id", "SaleNumber", "SaleDate", "CustomerId", "TotalAmount", "CancelationReason", "CancelationDate", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "CartId", "IsCancelled") FROM stdin;
\.


--
-- TOC entry 3113 (class 0 OID 16414)
-- Dependencies: 204
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: developer
--

COPY public."Users" ("Id", "Username", "Email", "Phone", "Password", "Role", "Status", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy") FROM stdin;
ff58a737-3eb5-4614-bf76-0ee01648423f	admin	admin@ambev.com	11999999999	$2a$11$UB3AZm7Hme7wVF7nn8z2LOvGx88Ly8Y..9DVg.1dKAa9Pa6vU2c0S	Admin	Active	2025-06-23 14:27:39.350349+00	2025-06-23 14:27:39.350349+00	seed	\N
a7dc3630-aee9-4044-8bd5-afb505a79203	joao	joao@email.com	11988887777	$2a$11$OBiwmn/fdUoK5lzKaEfo/.VSU0kfuFGZDtbtVYnyXb9nN3r8KVIP2	Customer	Active	2025-06-23 14:27:39.350349+00	2025-06-23 14:27:39.350349+00	seed	\N
63b79822-3071-4f98-8fb6-96f8a5b2e735	maria	maria@email.com	11977776666	$2a$11$5VVAQNRUjxCPMsTLw6MPD.7w4Z8xs870rJfDeBskEunvwPV6rUgDm	None	Active	2025-06-23 14:27:39.350349+00	2025-06-23 14:27:39.350349+00	seed	\N
\.


--
-- TOC entry 3110 (class 0 OID 16385)
-- Dependencies: 201
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: developer
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250622144951_Initial	9.0.6
\.


--
-- TOC entry 2965 (class 2606 OID 16432)
-- Name: CartProducts PK_CartProducts; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."CartProducts"
    ADD CONSTRAINT "PK_CartProducts" PRIMARY KEY ("Id");


--
-- TOC entry 2958 (class 2606 OID 16397)
-- Name: Carts PK_Carts; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."Carts"
    ADD CONSTRAINT "PK_Carts" PRIMARY KEY ("Id");


--
-- TOC entry 2960 (class 2606 OID 16405)
-- Name: Products PK_Products; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "PK_Products" PRIMARY KEY ("Id");


--
-- TOC entry 2973 (class 2606 OID 16532)
-- Name: SaleItems PK_SaleItems; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."SaleItems"
    ADD CONSTRAINT "PK_SaleItems" PRIMARY KEY ("Id");


--
-- TOC entry 2969 (class 2606 OID 16522)
-- Name: Sales PK_Sales; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."Sales"
    ADD CONSTRAINT "PK_Sales" PRIMARY KEY ("Id");


--
-- TOC entry 2962 (class 2606 OID 16424)
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- TOC entry 2956 (class 2606 OID 16389)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 2963 (class 1259 OID 16462)
-- Name: IX_CartProducts_ProductId; Type: INDEX; Schema: public; Owner: developer
--

CREATE INDEX "IX_CartProducts_ProductId" ON public."CartProducts" USING btree ("ProductId");


--
-- TOC entry 2970 (class 1259 OID 16543)
-- Name: IX_SaleItems_ProductId; Type: INDEX; Schema: public; Owner: developer
--

CREATE INDEX "IX_SaleItems_ProductId" ON public."SaleItems" USING btree ("ProductId");


--
-- TOC entry 2971 (class 1259 OID 16544)
-- Name: IX_SaleItems_SaleId; Type: INDEX; Schema: public; Owner: developer
--

CREATE INDEX "IX_SaleItems_SaleId" ON public."SaleItems" USING btree ("SaleId");


--
-- TOC entry 2967 (class 1259 OID 16523)
-- Name: IX_Sales_SaleNumber; Type: INDEX; Schema: public; Owner: developer
--

CREATE UNIQUE INDEX "IX_Sales_SaleNumber" ON public."Sales" USING btree ("SaleNumber");


--
-- TOC entry 2966 (class 1259 OID 16513)
-- Name: cartproducts_cartid_ix; Type: INDEX; Schema: public; Owner: developer
--

CREATE UNIQUE INDEX cartproducts_cartid_ix ON public."CartProducts" USING btree ("CartId", "ProductId");


--
-- TOC entry 2975 (class 2606 OID 32769)
-- Name: CartProducts FK_CartProducts_Carts_CartId; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."CartProducts"
    ADD CONSTRAINT "FK_CartProducts_Carts_CartId" FOREIGN KEY ("CartId") REFERENCES public."Carts"("Id") ON DELETE RESTRICT;


--
-- TOC entry 2976 (class 2606 OID 32774)
-- Name: CartProducts FK_CartProducts_Products_ProductId; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."CartProducts"
    ADD CONSTRAINT "FK_CartProducts_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES public."Products"("Id") ON DELETE RESTRICT;


--
-- TOC entry 2978 (class 2606 OID 32779)
-- Name: SaleItems FK_SaleItems_Products_ProductId; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."SaleItems"
    ADD CONSTRAINT "FK_SaleItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES public."Products"("Id") ON DELETE RESTRICT;


--
-- TOC entry 2979 (class 2606 OID 32784)
-- Name: SaleItems FK_SaleItems_Sales_SaleId; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."SaleItems"
    ADD CONSTRAINT "FK_SaleItems_Sales_SaleId" FOREIGN KEY ("SaleId") REFERENCES public."Sales"("Id") ON DELETE RESTRICT;


--
-- TOC entry 2974 (class 2606 OID 32794)
-- Name: Carts carts_users_fk; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."Carts"
    ADD CONSTRAINT carts_users_fk FOREIGN KEY ("UserId") REFERENCES public."Users"("Id");


--
-- TOC entry 2977 (class 2606 OID 24593)
-- Name: Sales sales_carts_fk; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public."Sales"
    ADD CONSTRAINT sales_carts_fk FOREIGN KEY ("CartId") REFERENCES public."Carts"("Id");


--
-- TOC entry 3123 (class 0 OID 0)
-- Dependencies: 5
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: developer
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2025-06-27 04:31:30

--
-- PostgreSQL database dump complete
--

--
-- Database "postgres" dump
--

\connect postgres

--
-- PostgreSQL database dump
--

-- Dumped from database version 13.21 (Debian 13.21-1.pgdg120+1)
-- Dumped by pg_dump version 17.5

-- Started on 2025-06-27 04:31:30

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: developer
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO developer;

--
-- TOC entry 3005 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: developer
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2025-06-27 04:31:30

--
-- PostgreSQL database dump complete
--

-- Completed on 2025-06-27 04:31:30

--
-- PostgreSQL database cluster dump complete
--

