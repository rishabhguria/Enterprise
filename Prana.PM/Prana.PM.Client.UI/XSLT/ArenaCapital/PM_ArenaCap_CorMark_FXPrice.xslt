<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="PositionMaster">
					<PositionMaster>
						
            
            <BaseCurrency>
              <xsl:value-of select ="'USD'"/>
            </BaseCurrency>

            <SettlementCurrency>
              <xsl:value-of select="COL3"/>
            </SettlementCurrency>


            <xsl:choose>
              <xsl:when test ="boolean(number(COL5))">
                <ForexPrice>
									<xsl:value-of select="(COL5)"/>
								</ForexPrice>
							</xsl:when >
							<xsl:otherwise>
								<ForexPrice>
									<xsl:value-of select="0"/>
								</ForexPrice>
							</xsl:otherwise>
						</xsl:choose >

						
						<xsl:choose>
							<xsl:when test="COL4 != '*' and COL4 != 'DX_DATE'">
								<Date>
									<xsl:value-of select="concat(substring(COL4,6,2),'/',substring(COL4,9,2),'/',substring(COL4,1,4))"/>
								</Date>
							</xsl:when>
							<xsl:otherwise>
									<Date>
										<xsl:value-of select="''"/>
									</Date>
								</xsl:otherwise>							
						</xsl:choose>
					</PositionMaster>			
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
