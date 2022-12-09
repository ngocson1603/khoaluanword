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
from fastapi import FastAPI, Response, status, HTTPException
from typing import Union
from pydantic import BaseModel
from random import randrange
app = FastAPI()


class Item(BaseModel):
    name: str
    price: str


my_post = [{"name": "son", "price": "abc", "id": 1}, {"name": "son", "price": "abc", "id": 2}, {
    "name": "son", "price": "abc", "id": 3}, {"name": "son", "price": "abc", "id": 4}]


def find_post(id):
    for p in my_post:
        if p['id'] == id:
            return p


def find_index_post(id):
    for i, p in enumerate(my_post):
        if p['id'] == id:
            return i


@app.put("/items/{item_id}")
def update_item(item_id: int, item: Item):
    return {"item_name": item.name, "item_id": item_id}


@app.post("/items", status_code=status.HTTP_201_CREATED)
def create_posts(post: Item):
    post_dict = post.dict()
    post_dict['id'] = randrange(0, 100000000)
    my_post.append(post_dict)
    return {"data": post_dict}


@app.get("/items")
def get_posts():
    return {"data": my_post}


@app.get("/items/lastes")
def get_lastest_post():
    post = my_post[len(my_post)-1]
    return {"detail": post}


@app.get("/items/{id}")
def get_post_id(id: int, response: Response):
    post = find_post(id)
    if not post:
        # raise HTTPException(status_code=status.HTTP_404_NOT_FOUND,
        #                     detail=f"post with id:{id} was not found")
        response.status_code = status.HTTP_404_NOT_FOUND
        return {'message': f"post with id:{id} was not found"}
    return {"data": post}


@app.delete("/items/{id}", status_code=status.HTTP_204_NO_CONTENT)
def delete_post(id: int):
    index = find_index_post(id)
    if index == None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND,
                            detail=f"post with id{id} does not exist")
    my_post.pop(index)
    return Response(status_code=status.HTTP_204_NO_CONTENT)


@app.put("/items/{id}")
def update_post(id: int, post: Item):
    index = find_index_post(id)
    if index == None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND,
                            detail=f"post with id{id} does not exist")

    post_dict = post.dict()
    post_dict['id'] = id
    my_post[index] = post_dict
    return {"data", post_dict}
