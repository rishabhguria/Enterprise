<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="COL1 != 'Symbol'">
          <PositionMaster>
            <Symbol>
              <xsl:value-of select="normalize-space(COL1)"/>
            </Symbol>

            <Symbology>
              <xsl:value-of select="normalize-space(COL2)"/>
            </Symbology>

            <!--<Text>
							<xsl:value-of select="normalize-space(COL2)"/>
						</Text>-->
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>