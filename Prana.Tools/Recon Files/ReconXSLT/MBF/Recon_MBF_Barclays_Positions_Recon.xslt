<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"	>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL1"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType ='EQUITY'">				
					
				<PositionMaster>
					<!--fundname section-->
					<FundName>
						<xsl:value-of select="''"/>
					</FundName>
					
					<xsl:if test="COL11 &lt; 0">
						<Side>
							<xsl:value-of select="'Sell'"/>
						</Side>
						<Quantity>
							<xsl:value-of select="COL11*(-1)"/>
						</Quantity>
					</xsl:if>
					<xsl:if test="COL11 &gt; 0 and COL11 != '0' ">
						<Side>
							<xsl:value-of select="'Buy'"/>
						</Side>
						<Quantity>
							<xsl:value-of select="COL11"/>
						</Quantity>
					</xsl:if>

					<AvgPX>
						<xsl:value-of select="COL13"/>
						<!--<xsl:value-of select="COL14"/>-->
					</AvgPX>
					
						<!-- Symbol Section-->
					
					<PBSymbol>
						<xsl:value-of select="COL3"/>
					</PBSymbol>

					<CompanyName>
						<xsl:value-of select="COL4"/>
					</CompanyName>
					
					<Symbol>
						<xsl:value-of select="''"/>
					</Symbol>
					
					<RIC>
						<xsl:value-of select ="COL3"/>
					</RIC>

				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
