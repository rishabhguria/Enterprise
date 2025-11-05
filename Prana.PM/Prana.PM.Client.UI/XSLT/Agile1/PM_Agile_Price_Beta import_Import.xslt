<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="Beta">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL17)"/>
					</xsl:call-template>
				</xsl:variable>
				<!--<xsl:variable name="Beta">
					<xsl:choose>
						<xsl:when test="contains(COL13,'Option')">
							<xsl:value-of select="COL14"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL17"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>-->

				<xsl:if test ="number($Beta) and not(contains(COL1,'CURNCY'))">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Agile'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name ="Symbol">
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>-->

						<!--<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL1!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>-->
						<!--<xsl:variable name="Bloomberg">
							<xsl:choose>
								<xsl:when test="contains(substring(substring-after(substring-after(substring-after(normalize-space(COL1 ),'/'),'/'),' '),1,1),'P') or contains(substring(substring-after(substring-after(substring-after(normalize-space(COL1 ),'/'),'/'),' '),1,1),'C')">
									<xsl:value-of select="normalize-space(COL1)"/>
								</xsl:when>
								<xsl:when test="contains(COL1,' ')">
									<xsl:value-of select="normalize-space(COL1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat(substring-before(COL1,' '),' ','US EQUITY')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable> 

						<Bloomberg>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL1!=''">
									<xsl:value-of select="COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>-->

						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<Bloomberg>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL1)"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>
						<Symbology>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="'Symbol'"/>
								</xsl:when>

								<xsl:when test="COL1!='*'">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'Symbol'"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbology>

						<Beta>
							<xsl:choose>
								<xsl:when test="number($Beta)">
									<xsl:value-of select="$Beta"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>

							</xsl:choose>
						</Beta>

						<Date>
							<xsl:value-of select ="''"/>
						</Date>

						

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
