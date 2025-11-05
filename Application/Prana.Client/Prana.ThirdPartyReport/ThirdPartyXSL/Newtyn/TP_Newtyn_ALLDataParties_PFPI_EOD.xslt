<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
          
			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty = 'PFPI']">
				

				<ThirdPartyFlatFileDetail>					
					
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>
					
					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

					<AccountID>
					 <xsl:value-of select ="'NEWTYN'"/>
					</AccountID>
					
					<SubAcct>
					 <xsl:value-of select ="AccountName"/>
					</SubAcct>
					
					<Security>
					 <xsl:value-of select ="Symbol"/>
					</Security>
					
				<Side>
					<xsl:choose>
						<xsl:when test="Side='Buy to Open' or Side='Buy' ">							
								<xsl:value-of select ="'B'"/>						
						</xsl:when>						
						<xsl:when test="Side='Sell' or Side='Sell to Close' ">						
								<xsl:value-of select ="'S'"/>						
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open' ">						
								<xsl:value-of select ="'SS'"/>							
						</xsl:when>
						<xsl:when test="Side='Buy to Close'">							
								<xsl:value-of select ="'BC'"/>						
						</xsl:when>	
						<xsl:otherwise>							
								<xsl:value-of select="Side"/>							
						</xsl:otherwise>
					</xsl:choose>
				</Side>
				
					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

								
					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>
					
					<Comm>
					<xsl:value-of select="format-number(CommissionPerShare,'#.####')"/>
					</Comm>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
