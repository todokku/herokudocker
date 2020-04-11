-- Table: public."Videos"

-- DROP TABLE public."Videos";

CREATE TABLE public."Videos"
(
    "Id" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "CreatedAt" timestamp without time zone,
    "EmbedCode" text COLLATE pg_catalog."default",
    "Desc" text COLLATE pg_catalog."default",
    CONSTRAINT "Videos_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public."Videos"
    OWNER to slppjedhxbbwnk;