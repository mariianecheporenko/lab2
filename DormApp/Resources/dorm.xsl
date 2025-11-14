<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" encoding="utf-8" indent="yes"/>

  <xsl:template match="/DormResidents">
    <html>
      <head>
        <meta charset="utf-8"/>
        <title>Гуртожиток — Residents</title>
      </head>
      <body>
        <h1>Список мешканців гуртожитку</h1>
        <table border="1" cellpadding="6" cellspacing="0">
          <tr>
            <th>П.І.П.</th><th>Факультет</th><th>Кафедра</th><th>Курс</th><th>Кімната</th><th>Дата поселення</th>
          </tr>
          <xsl:for-each select="Resident">
            <tr>
              <td><xsl:value-of select="FullName"/></td>
              <td><xsl:value-of select="@faculty"/></td>
              <td><xsl:value-of select="Chair"/></td>
              <td><xsl:value-of select="Course"/></td>
              <td><xsl:value-of select="@room"/></td>
              <td><xsl:value-of select="@movedIn"/></td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
