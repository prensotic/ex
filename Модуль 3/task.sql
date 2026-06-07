SELECT
    SUM(p.price * o.quentity) AS итог
FROM orders o
JOIN products p
    ON p.id = o.product_id
GROUP BY o.order_id;