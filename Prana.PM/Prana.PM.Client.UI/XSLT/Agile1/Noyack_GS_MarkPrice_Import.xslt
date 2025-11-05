<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name ="MarkPrice">
					<xsl:value-of select="number(COL10)"/>
				</xsl:variable>

				<xsl:if test ="number(COL7) ">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL5"/>
						</xsl:variable>

						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select ="COL12"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL5,'.')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varExFactor">
							<xsl:value-of select="substring(COL5,string-length(substring-before(COL5,'.')),1)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL5!=''">
									<xsl:choose>

										<xsl:when test="$varExFactor='a'">
											<xsl:value-of select="concat(substring(COL5,1,string-length(substring-before(COL5,'.'))-1),'_A',$PRANA_SUFFIX_NAME)"/>
										</xsl:when>

										<xsl:when test="$varExFactor='b'">
											<xsl:value-of select="concat(substring(COL5,1,string-length(substring-before(COL5,'.'))-1),'_B',$PRANA_SUFFIX_NAME)"/>
										</xsl:when>

										<xsl:when test="$PRANA_SUFFIX_NAME!=''">
											<xsl:value-of select="concat(substring-before(COL5,'.'),$PRANA_SUFFIX_NAME)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="COL5"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<Date>
							<xsl:value-of select="''"/>
						</Date>						

						<MarkPrice>
							<xsl:choose>

								<xsl:when test ="$MarkPrice &lt;0">
									<xsl:value-of select ="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$MarkPrice &gt;0">
									<xsl:value-of select ="$MarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
