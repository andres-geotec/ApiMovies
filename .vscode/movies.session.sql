SELECT *
FROM "categorys";

INSERT INTO "movies"("Name", "ImageRout", "Description", "Duration", "Classification", "Timestamp", "categoryId")
VALUES('Terminator', 'test', 'test description', 120, 1, '2020-08-31T00:18:00', 4);

SELECT *
FROM "movies";

SELECT *
FROM "users";



-- CREATE
CREATE TABLE products (
  id    int,
  name  varchar(100),
  geom  geometry(Point, 4326)
);

INSERT INTO products (id, name, geom)
VALUES
  (1, 'Wheel', ST_POINT(0.0, 0.0)),
  (2, 'Chela', ST_POINT(1.0, 1.0));

SELECT id, name, ST_ASTEXT(geom)
FROM products;

--DROP TABLE products;
