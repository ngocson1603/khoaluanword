# # Three lines to make our compiler able to draw:
# from sklearn.cluster import KMeans
# import matplotlib.pyplot as plt
# import sys
# import matplotlib
# matplotlib.use('Agg')


# x = [4, 5, 10, 4, 3, 11, 14, 6, 10, 12]
# y = [21, 19, 24, 17, 16, 25, 24, 22, 21, 21]

# data = list(zip(x, y))
# inertias = []

# for i in range(1, 11):
#     kmeans = KMeans(n_clusters=i)
#     kmeans.fit(data)
#     inertias.append(kmeans.inertia_)

# plt.plot(range(1, 11), inertias, marker='o')
# plt.title('Elbow method')
# plt.xlabel('Number of clusters')
# plt.ylabel('Inertia')
# plt.show()

# # Two  lines to make our compiler able to draw:
# plt.savefig(sys.stdout.buffer)
# sys.stdout.flush()
from fastapi import FastAPI
from typing import Union
from pydantic import BaseModel
app = FastAPI()


class Item(BaseModel):
    name: str
    price: float
    is_offer: Union[bool, None] = None


@app.get("/")
def read_root():
    return {"Hello": "World"}


@app.get("/post")
def get_posts():
    return {"Hello": "hello World"}


@app.put("/items/{item_id}")
def update_item(item_id: int, item: Item):
    return {"item_name": item.name, "item_id": item_id}


@app.post("/items")
def create_posts(item_id: int, item: Item):
    return {"item_name": item.name, "item_id": item_id}
