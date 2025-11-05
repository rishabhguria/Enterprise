<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">

				<xsl:if test="number(COL4) and contains(COL1,'CURNCY')">
					<PositionMaster>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(substring-before(COL1,'CURNCY'))"/>
						</xsl:variable>

						<xsl:variable name="BaseCurrency">
							<xsl:value-of select ="normalize-space(substring($varSymbol,1,3))"/>
						</xsl:variable>

						<BaseCurrency>
							<xsl:value-of select ="normalize-space(substring($varSymbol,1,3))"/>
						</BaseCurrency>

						<xsl:variable name="SettlementCurrency">
							<xsl:choose>
								<xsl:when test="normalize-space(substring($varSymbol,4,3))=''">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(substring($varSymbol,4,3))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<SettlementCurrency>
							<xsl:choose>
								<xsl:when test="normalize-space(substring($varSymbol,4,3))=''">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(substring($varSymbol,4,3))"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlementCurrency>

						<xsl:choose>
							<xsl:when test ="number(COL4)">
								<ForexPrice>
									<xsl:value-of select="COL4"/>
								</ForexPrice>
							</xsl:when >
							<xsl:otherwise>
								<ForexPrice>
									<xsl:value-of select="0"/>
								</ForexPrice>
							</xsl:otherwise>
						</xsl:choose >
						<Date>
							<xsl:value-of select="''"/>
						</Date>

						<FXConversionMethodOperator>
							<xsl:choose>
								<xsl:when test="$BaseCurrency='EUR' or $BaseCurrency='AUD' or $BaseCurrency='GBP' or $BaseCurrency='NZD'  or $BaseCurrency='JPY' or $BaseCurrency='HKD' ">
									<xsl:value-of select="'M'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'D'"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select="'M'"/>-->
						</FXConversionMethodOperator>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
