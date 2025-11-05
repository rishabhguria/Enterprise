<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>


					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<Account>
						<xsl:value-of select ="AccountName"/>
					</Account>


					<xsl:variable name="varMonth">
						<xsl:value-of select="substring-before(substring-after(TradeDate,'-'),'-')"/>
					</xsl:variable>

					<xsl:variable name="varDay">
						<xsl:value-of select="substring-before(substring-after(substring-after(TradeDate,'-'),'-'),'T')"/>
					</xsl:variable>

					<xsl:variable name="varYear">
						<xsl:value-of select="substring-before(TradeDate,'-')"/>
					</xsl:variable>

					<Tradedate>
						<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
					</Tradedate>
					

					<Symbol>
						<xsl:value-of select ="Symbol"/>
					</Symbol>

					<CashValueLocal>
						<xsl:value-of select="CashValueLocal"/>
					</CashValueLocal>

					<CashValueBase>
						<xsl:value-of select="CashValueBase"/>
					</CashValueBase>



					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	
</xsl:stylesheet>
