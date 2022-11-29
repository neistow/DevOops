{{- define "elastic.labels" -}}
{{ include "app.labels" . }}
app.kubernetes.io/component: elastic
{{- end }}

{{- define "elastic.selectorLabels" -}}
{{ include "app.selectorLabels" . }}
app.kubernetes.io/component: elastic
{{- end }}

{{- define "elastic.url" -}}
{{- $appFullname := include "app.fullname" . -}}
http://{{ $appFullname }}-elastic-0.{{ $appFullname }}-elastic:9200
{{- end }}