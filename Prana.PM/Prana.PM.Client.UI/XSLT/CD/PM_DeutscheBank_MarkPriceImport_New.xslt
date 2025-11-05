<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<!-- Put = 0,Call = 1 , Here First call/put code then 2 characters for month code -->
		<!-- Call month Codes e.g. 101 represents 1=Call, 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth=101">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=102">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=103">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=104">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=105">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=106">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=107">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=108">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=109">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=110">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=111">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=112">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<!-- Put month Codes e.g. 001 represents 0=Put, 01 = January-->
			<xsl:when test ="$varMonth=001">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=002">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=003">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=004">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=005">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=006">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=007">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=008">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=009">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=010">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=011">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=012">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Month">
		<xsl:param name="varMonth"/>
		<!-- 2 characters for month code -->
		<!-- month Codes e.g. 01 represents 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth='A'">
				<xsl:value-of select ="'01'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='B'">
				<xsl:value-of select ="'02'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='C'">
				<xsl:value-of select ="'03'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='D'">
				<xsl:value-of select ="'04'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='E'">
				<xsl:value-of select ="'05'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='F'">
				<xsl:value-of select ="'06'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='G'">
				<xsl:value-of select ="'07'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='H'">
				<xsl:value-of select ="'08'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='I'">
				<xsl:value-of select ="'09'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='J'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='K'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='L'">
				<xsl:value-of select ="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
		
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="(normalize-space(COL11)= 'EQ' or normalize-space(COL11)= 'PS' or normalize-space(COL11) = 'B') and contains(COL6,'Topwater Investment') !=false">
					<PositionMaster>

						<xsl:variable name="varSecurityType" select="COL12"/>
						
						<xsl:variable name ="varSymbol">
							<xsl:choose>
								<xsl:when test ="normalize-space(COL11)= 'B' ">
									<xsl:value-of select ="COL16"/>
								</xsl:when>
								
								<xsl:when test ="contains(COL14,'.TO')!=False and normalize-space(COL11)!= 'B' ">
									<xsl:value-of select ="concat(substring-before(COL14,'.'),'-TC')"/>
								</xsl:when>
								<xsl:when test ="contains(COL14,'.TO')=False and normalize-space(COL11)!= 'B'">
									<xsl:value-of select ="substring-before(COL14,'.')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varOptionString" select="substring-before(COL14,'.')"/>


						<xsl:variable name ="varPBSymbol">
							<xsl:choose>
								
								<xsl:when test ="contains($varSymbol,'_')!=False ">
									<xsl:value-of select ="concat(substring-before($varSymbol,'_'),'/',translate(substring-after($varSymbol,'_'),$vLowercaseChars_CONST,$vUppercaseChars_CONST))"/>
								</xsl:when>
								<xsl:when test ="contains($varSymbol,'_')=False">
									<xsl:value-of select ="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varPBSymbol_Length" select="string-length($varPBSymbol)"/>

						<xsl:variable name="varOption_StrikePrice" select="concat(number(substring($varPBSymbol,$varPBSymbol_Length -4,3)),'.',substring($varPBSymbol,$varPBSymbol_Length -1,2))"/>

						<xsl:variable name="varOption_Year" select="substring($varPBSymbol,$varPBSymbol_Length -6,2)"/>

						<xsl:variable name="varMonthCode" select="substring($varPBSymbol,$varPBSymbol_Length -9,1)"/>
						
						
						<xsl:variable name = "varOption_ExpirationMonth" >
							<xsl:call-template name="Month">
								<xsl:with-param name="varMonth" select="$varMonthCode" />
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varOption_Underlying" select="substring($varPBSymbol,1,$varPBSymbol_Length -10)"/>


						<!--Call/Put code for  month code for Equity Option-->
						<xsl:variable name ="varCallPutCode">
							<xsl:choose>
								<xsl:when test ="normalize-space(COL13)='C'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL13)='P'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						
						<xsl:variable name = "varOption_MonthCode" >
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="concat($varCallPutCode,$varOption_ExpirationMonth)" />
							</xsl:call-template>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varSecurityType='Equity Option'">
								<Symbol>
									<xsl:value-of select ="concat('O:',$varOption_Underlying,' ',$varOption_Year,$varMonthCode,$varOption_StrikePrice)"/>
								</Symbol>
								
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test ="$varSecurityType !='Equity Option'">
								<Symbol>
									<xsl:value-of select ="$varPBSymbol"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>
						
						
							<xsl:choose>
								<xsl:when  test="boolean(number(COL29))">
									<MarkPrice>
										<xsl:value-of select="COL29"/>
									</MarkPrice>
								</xsl:when >
								<xsl:otherwise>
									<MarkPrice>
										<xsl:value-of select="0"/>
									</MarkPrice>
								</xsl:otherwise>
							</xsl:choose >
					
						
								<Date>
									<xsl:value-of select="''"/>
								</Date>
							

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->

</xsl:stylesheet>


