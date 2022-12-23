import requests


def greeting():

    # Make a GET request to the API
    response = requests.get('http://localhost:3000/api/sanpham')

    # Check the status code of the response
    if response.status_code == 200:
        # If the status code is 200, the request was successful
        # and the data is available in the response object
        data = response.json()
        # Extract the list you want from the data
        my_list = []
        price = []
        for item in data:
            my_list.append(item['pro_name'])
            price.append(item['price'])
        # You can now work with the list
        print(my_list, price)
    else:
        # If the status code is not 200, there was an error
        # You can print the error message to troubleshoot
        print(response.text)


greeting()
