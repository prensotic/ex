USE combine;

WITH ComponentCost AS (
    SELECT 
        pc.product_id,
        SUM(c.price * pc.components_quentity) AS comp_sum
    FROM ProductsComponents pc
    JOIN Components c ON c.id = pc.component_id
    GROUP BY pc.product_id
),
ProductCost AS (
    SELECT 
        p.id,
        p.price + ISNULL(cc.comp_sum, 0) AS full_price
    FROM Products p
    LEFT JOIN ComponentCost cc ON cc.product_id = p.id
)
SELECT 
    SUM(o.quentity * pc.full_price) AS total_sum
FROM Orders o
JOIN ProductCost pc ON pc.id = o.product_id
WHERE o.order_id = 1;