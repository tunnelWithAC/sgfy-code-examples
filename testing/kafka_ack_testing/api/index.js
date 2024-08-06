const express = require('express');
const app = express();
app.use(express.json());

let items = [];

app.get('/api', (req, res) => {
    res.json(items);
});

app.post('/api', (req, res) => {
    const item = req.body;
    items.push(item);
    res.status(201).json(item);
});

const port = process.env.PORT || 3000;
app.listen(port, () => console.log(`Server is listening on port ${port}`));
