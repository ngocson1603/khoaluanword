from django.shortcuts import render
from django.http import HttpResponse
from django.template import loader
# Create your views here.


def get_home(request):
    return render(request, 'home.html')


def testing(request):
    template = loader.get_template('home.html')
    context = {
        'firstname': 'son',
    }
    return HttpResponse(template.render(context, request))
