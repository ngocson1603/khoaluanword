from django.http import HttpResponse
from django.template import loader


def tesing(request):
    template = loader.get_template('django.html')
    context = {
        'firstname': 'son'
    }
    return HttpResponse(template.render(context, request))
