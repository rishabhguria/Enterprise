<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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
	</xsl:template>

	<xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<xsl:template name="UPPER">
		<xsl:param name="text"/>
		<xsl:value-of select="translate(COL2, $smallcase, $uppercase)"/>
	</xsl:template>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varMarkPrice">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL7)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varMarkPrice) and not(contains(COL2,'CORP'))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>					

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						
						<xsl:variable name="varBloomberg">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>							
								<xsl:when test ="$varBloomberg !=''">
									<xsl:value-of select ="$varBloomberg"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
						<Bloomberg>
							<xsl:choose>													
								<xsl:when test ="$varBloomberg !=''">
									<xsl:value-of select ="$varBloomberg"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>
						
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPrice &gt; 0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test="$varMarkPrice &lt; 0">
									<xsl:value-of select="$varMarkPrice * (1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name = "varDate">
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select ="$varDate"/>
						</Date>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>